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
            InitializeComponent()
'#Region "Data binding"
            Me.list = New System.ComponentModel.BindingList(Of DragAndDrop_ChangeGroup.TestData)()
            For i As Integer = 0 To 100 - 1
                Me.list.Add(New DragAndDrop_ChangeGroup.TestData() With {.Text = "row " & i, .Group = "group " & i \ 5, .Number = i})
            Next

            grid.ItemsSource = Me.list
            grid.ExpandAllGroups()
'#End Region
            view.AllowDrop = True
             ''' Cannot convert AssignmentExpressionSyntax, System.NullReferenceException: Object reference not set to an instance of an object.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAssignmentExpression(AssignmentExpressionSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
''' 
'''             view.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.View_PreviewMouseDown)
'''   ''' Cannot convert AssignmentExpressionSyntax, System.NullReferenceException: Object reference not set to an instance of an object.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAssignmentExpression(AssignmentExpressionSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''             view.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(this.View_PreviewMouseMove)
'''   ''' Cannot convert AssignmentExpressionSyntax, System.NullReferenceException: Object reference not set to an instance of an object.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAssignmentExpression(AssignmentExpressionSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''             view.DragOver += new System.Windows.DragEventHandler(this.View_DragOver)
'''   ''' Cannot convert AssignmentExpressionSyntax, System.NullReferenceException: Object reference not set to an instance of an object.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAssignmentExpression(AssignmentExpressionSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''             view.Drop += new System.Windows.DragEventHandler(this.View_Drop)
'''  End Sub

        Private Sub View_Drop(ByVal sender As Object, ByVal e As System.Windows.DragEventArgs)
            Dim rowHandle As Integer = CInt(e.Data.GetData(GetType(Integer)))
            Dim obj As DragAndDrop_ChangeGroup.TestData = CType(grid.GetRow(rowHandle), DragAndDrop_ChangeGroup.TestData)
            Dim dropRowHandle As Integer = view.GetRowHandleByTreeElement(TryCast(e.OriginalSource, System.Windows.DependencyObject))
            If grid.GroupCount = 1 Then
                Dim fieldName As String = grid.SortInfo(CInt((0))).FieldName
                Dim groupValue As Object = grid.GetCellValue(dropRowHandle, fieldName)
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
            If view.GetRowElementByTreeElement(TryCast(e.OriginalSource, System.Windows.DependencyObject)) IsNot Nothing AndAlso grid.GroupCount = 1 Then
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
            Dim rowHandle As Integer = view.GetRowHandleByMouseEventArgs(e)
            If Me.dragStarted Then
                Dim data As System.Windows.DataObject = Me.CreateDataObject(rowHandle)
                Call System.Windows.DragDrop.DoDragDrop(view.GetRowElementByMouseEventArgs(e), data, System.Windows.DragDropEffects.Move Or System.Windows.DragDropEffects.Copy)
                Me.dragStarted = False
            End If
        End Sub

        Private Sub View_PreviewMouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
            Dim rowHandle As Integer = view.GetRowHandleByMouseEventArgs(e)
            If rowHandle <> GridDataController.InvalidRow AndAlso Not grid.IsGroupRowHandle(rowHandle) Then Me.dragStarted = True
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
