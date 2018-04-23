Imports Microsoft.VisualBasic
Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Input
Imports DevExpress.Data

Namespace DragAndDrop_ChangeGroup

	Partial Public Class Window1
		Inherits Window
		Private list As BindingList(Of TestData)
		Private dragStarted As Boolean

		Public Sub New()
			InitializeComponent()

'			#Region "Data binding"
			list = New BindingList(Of TestData)()
			For i As Integer = 0 To 99
				list.Add(New TestData() With {.Text = "row " & i, .Group = "group " & i \ 5, .Number = i})
			Next i

			grid.ItemsSource = list
			grid.ExpandAllGroups()
'			#End Region

			view.AllowDrop = True

			AddHandler view.PreviewMouseDown, AddressOf View_PreviewMouseDown
			AddHandler view.PreviewMouseMove, AddressOf View_PreviewMouseMove
			AddHandler view.DragOver, AddressOf View_DragOver
			AddHandler view.Drop, AddressOf View_Drop

		End Sub

		Private Sub View_Drop(ByVal sender As Object, ByVal e As DragEventArgs)
			Dim rowHandle As Integer = CInt(Fix(e.Data.GetData(GetType(Integer))))
			Dim obj As TestData = CType(grid.GetRow(rowHandle), TestData)
			Dim dropRowHandle As Integer = view.GetRowHandleByTreeElement(TryCast(e.OriginalSource, DependencyObject))
			If grid.GroupCount = 1 Then
				Dim fieldName As String = grid.SortInfo(0).FieldName
				Dim groupValue As Object = grid.GetCellValue(dropRowHandle, fieldName)
				If IsCopyEffect(e) Then
					Dim newData As New TestData() With {.Text = obj.Text & " (Copy)", .Number = obj.Number, .Group = obj.Group}
					TypeDescriptor.GetProperties(GetType(TestData))(fieldName).SetValue(newData, groupValue)
					list.Add(newData)
				Else
					TypeDescriptor.GetProperties(GetType(TestData))(fieldName).SetValue(obj, groupValue)
				End If
			End If
		End Sub

		Private Sub View_DragOver(ByVal sender As Object, ByVal e As DragEventArgs)
			If view.GetRowElementByTreeElement(TryCast(e.OriginalSource, DependencyObject)) IsNot Nothing AndAlso grid.GroupCount = 1 Then
				e.Effects = If(IsCopyEffect(e), DragDropEffects.Copy, DragDropEffects.Move)
			Else
				e.Effects = DragDropEffects.None
			End If
			e.Handled = True
		End Sub

		Private Function IsCopyEffect(ByVal e As DragEventArgs) As Boolean
			Return (e.KeyStates And DragDropKeyStates.ControlKey) = DragDropKeyStates.ControlKey
		End Function

		Private Sub View_PreviewMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim rowHandle As Integer = view.GetRowHandleByMouseEventArgs(e)
			If dragStarted Then
				Dim data As DataObject = CreateDataObject(rowHandle)
				DragDrop.DoDragDrop(view.GetRowElementByMouseEventArgs(e), data, DragDropEffects.Move Or DragDropEffects.Copy)
				dragStarted = False
			End If
		End Sub

		Private Sub View_PreviewMouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			Dim rowHandle As Integer = view.GetRowHandleByMouseEventArgs(e)
			If rowHandle <> GridDataController.InvalidRow AndAlso (Not grid.IsGroupRowHandle(rowHandle)) Then
				dragStarted = True
			End If
		End Sub

		Private Function CreateDataObject(ByVal rowHandle As Integer) As DataObject
			Dim data As New DataObject()
			data.SetData(GetType(Integer), rowHandle)
			Return data
		End Function
	End Class

	#Region "TestData class"
	Public Class TestData
		Implements INotifyPropertyChanged
		Private text_Renamed As String
		Private number_Renamed As Integer
		Private group_Renamed As String

		Public Property Text() As String
			Get
				Return text_Renamed
			End Get
			Set(ByVal value As String)
				If Text = value Then
					Return
				End If
				text_Renamed = value
				OnPorpertyChanged("Text")
			End Set
		End Property
		Public Property Number() As Integer
			Get
				Return number_Renamed
			End Get
			Set(ByVal value As Integer)
				If Number = value Then
					Return
				End If
				number_Renamed = value
				OnPorpertyChanged("Number")
			End Set

		End Property
		Public Property Group() As String
			Get
				Return group_Renamed
			End Get
			Set(ByVal value As String)
				If Group = value Then
					Return
				End If
				group_Renamed = value
				OnPorpertyChanged("Group")
			End Set
		End Property

		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
		Private Sub OnPorpertyChanged(ByVal propertyName As String)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
		End Sub
	End Class
	#End Region
End Namespace