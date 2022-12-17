Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Input
Imports DevExpress.Data

Namespace DragAndDrop_ChangeGroup

    Public Partial Class Window1
        Inherits System.Windows.Window

        Private list As System.ComponentModel.BindingList(Of DragAndDrop_ChangeGroup.TestData)

        Private dragStarted As Boolean

        Public Sub New()
            Me.InitializeComponent()
'#Region "Data binding"
            Me.list = New System.ComponentModel.BindingList(Of DragAndDrop_ChangeGroup.TestData)()
            For i As Integer = 0 To 100 - 1
                Me.list.Add(New DragAndDrop_ChangeGroup.TestData() With {.Text = "row " & i, .Group = "group " & i \ 5, .Number = i})
            Next

            Me.grid.ItemsSource = Me.list
            Me.grid.ExpandAllGroups()
'#End Region
            Me.view.AllowDrop = True
            AddHandler Me.view.PreviewMouseDown, New System.Windows.Input.MouseButtonEventHandler(AddressOf Me.View_PreviewMouseDown)
            AddHandler Me.view.PreviewMouseMove, New System.Windows.Input.MouseEventHandler(AddressOf Me.View_PreviewMouseMove)
            AddHandler Me.view.DragOver, New System.Windows.DragEventHandler(AddressOf Me.View_DragOver)
            AddHandler Me.view.Drop, New System.Windows.DragEventHandler(AddressOf Me.View_Drop)
        End Sub

        Private Sub View_Drop(ByVal sender As Object, ByVal e As System.Windows.DragEventArgs)
            Dim rowHandle As Integer = CInt(e.Data.GetData(GetType(Integer)))
            Dim obj As DragAndDrop_ChangeGroup.TestData = CType(Me.grid.GetRow(rowHandle), DragAndDrop_ChangeGroup.TestData)
            Dim dropRowHandle As Integer = Me.view.GetRowHandleByTreeElement(TryCast(e.OriginalSource, System.Windows.DependencyObject))
            If Me.grid.GroupCount = 1 Then
                Dim fieldName As String = Me.grid.SortInfo(CInt((0))).FieldName
                Dim groupValue As Object = Me.grid.GetCellValue(dropRowHandle, fieldName)
                If Me.IsCopyEffect(e) Then
                    Dim newData As DragAndDrop_ChangeGroup.TestData = New DragAndDrop_ChangeGroup.TestData() With {.Text = obj.Text & " (Copy)", .Number = obj.Number, .Group = obj.Group}
                    Call System.ComponentModel.TypeDescriptor.GetProperties(CType((GetType(DragAndDrop_ChangeGroup.TestData)), System.Type))(CStr((fieldName))).SetValue(newData, groupValue)
                    Me.list.Add(newData)
                Else
                    Call System.ComponentModel.TypeDescriptor.GetProperties(CType((GetType(DragAndDrop_ChangeGroup.TestData)), System.Type))(CStr((fieldName))).SetValue(obj, groupValue)
                End If
            End If
        End Sub

        Private Sub View_DragOver(ByVal sender As Object, ByVal e As System.Windows.DragEventArgs)
            If Me.view.GetRowElementByTreeElement(TryCast(e.OriginalSource, System.Windows.DependencyObject)) IsNot Nothing AndAlso Me.grid.GroupCount = 1 Then
                e.Effects = If(Me.IsCopyEffect(e), System.Windows.DragDropEffects.Copy, System.Windows.DragDropEffects.Move)
            Else
                e.Effects = System.Windows.DragDropEffects.None
            End If

            e.Handled = True
        End Sub

        Private Function IsCopyEffect(ByVal e As System.Windows.DragEventArgs) As Boolean
            Return(e.KeyStates And System.Windows.DragDropKeyStates.ControlKey) = System.Windows.DragDropKeyStates.ControlKey
        End Function

        Private Sub View_PreviewMouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs)
            Dim rowHandle As Integer = Me.view.GetRowHandleByMouseEventArgs(e)
            If Me.dragStarted Then
                Dim data As System.Windows.DataObject = Me.CreateDataObject(rowHandle)
                Call System.Windows.DragDrop.DoDragDrop(Me.view.GetRowElementByMouseEventArgs(e), data, System.Windows.DragDropEffects.Move Or System.Windows.DragDropEffects.Copy)
                Me.dragStarted = False
            End If
        End Sub

        Private Sub View_PreviewMouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
            Dim rowHandle As Integer = Me.view.GetRowHandleByMouseEventArgs(e)
            If rowHandle <> DevExpress.Data.GridDataController.InvalidRow AndAlso Not Me.grid.IsGroupRowHandle(rowHandle) Then Me.dragStarted = True
        End Sub

        Private Function CreateDataObject(ByVal rowHandle As Integer) As DataObject
            Dim data As System.Windows.DataObject = New System.Windows.DataObject()
            data.SetData(GetType(Integer), rowHandle)
            Return data
        End Function
    End Class

'#Region "TestData class"
    Public Class TestData
        Implements System.ComponentModel.INotifyPropertyChanged

        Private textField As String

        Private numberField As Integer

        Private groupField As String

        Public Property Text As String
            Get
                Return Me.textField
            End Get

            Set(ByVal value As String)
                If Equals(Me.Text, value) Then Return
                Me.textField = value
                Me.OnPorpertyChanged("Text")
            End Set
        End Property

        Public Property Number As Integer
            Get
                Return Me.numberField
            End Get

            Set(ByVal value As Integer)
                If Me.Number = value Then Return
                Me.numberField = value
                Me.OnPorpertyChanged("Number")
            End Set
        End Property

        Public Property Group As String
            Get
                Return Me.groupField
            End Get

            Set(ByVal value As String)
                If Equals(Me.Group, value) Then Return
                Me.groupField = value
                Me.OnPorpertyChanged("Group")
            End Set
        End Property

        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements Global.System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Private Sub OnPorpertyChanged(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
'#End Region
End Namespace
