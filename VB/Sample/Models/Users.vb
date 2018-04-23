Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Web
Imports System.Web.SessionState

Namespace Sample.Models
	Public Class User
		Private id_Renamed As Integer
		Private nickname_Renamed As String
		Private avatarUrl_Renamed As String

		Public Sub New()
			ID = 0
			NickName = String.Empty
			AvatarUrl = String.Empty
		End Sub

		<Key> _
		Public Property ID() As Integer
			Get
				Return id_Renamed
			End Get
			Set(ByVal value As Integer)
				id_Renamed = value
			End Set
		End Property
		<Required(ErrorMessage := "NickName is required")> _
		Public Property NickName() As String
			Get
				Return nickname_Renamed
			End Get
			Set(ByVal value As String)
				nickname_Renamed = value
			End Set
		End Property
		Public Property AvatarUrl() As String
			Get
				Return avatarUrl_Renamed
			End Get
			Set(ByVal value As String)
				avatarUrl_Renamed = value
			End Set
		End Property
		Public Sub Assign(ByVal source As User)
			NickName = source.NickName
			AvatarUrl = source.AvatarUrl
		End Sub
	End Class

	Public NotInheritable Class UserProvider
		Private Const Key As String = "UserProvider"

		Private Sub New()
		End Sub
		Private Shared ReadOnly Property Session() As HttpSessionState
			Get
				Return HttpContext.Current.Session
			End Get
		End Property
		Private Shared ReadOnly Property Data() As List(Of User)
			Get
				If Session(Key) Is Nothing Then
					Restore()
				End If
				Return TryCast(Session(Key), List(Of User))
			End Get
		End Property

		Public Shared Function [Select]() As IEnumerable(Of User)
			Return Data
		End Function
		Public Shared Sub Insert(ByVal item As User)
			item.ID = Data.Count + 1
			Data.Add(item)
		End Sub
		Public Shared Sub Update(ByVal item As User)
			Dim storedItem As User = FindItem(item.ID)
			storedItem.Assign(item)
		End Sub
		Public Shared Sub Delete(ByVal item As User)
			Dim storedItem As User = FindItem(item.ID)
			Data.Remove(storedItem)
		End Sub
		Public Shared Sub Restore()
			Session(Key) = New List(Of User)()
		End Sub
		Private Shared Function FindItem(ByVal id As Integer) As User
			For Each item As User In Data
				If item.ID = id Then
					Return item
				End If
			Next item
			Return Nothing
		End Function
	End Class
End Namespace