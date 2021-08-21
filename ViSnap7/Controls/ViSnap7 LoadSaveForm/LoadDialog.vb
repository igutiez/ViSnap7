Imports System.Windows.Forms

Public Class LoadDialog
    Public folder As String
    Public extension As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        LoadRecipe(RecipeList.SelectedItem, Me.folder, Me.extension)
        Me.Close()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub RecipeList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RecipeList.SelectedIndexChanged
        LoadRecipe(RecipeList.SelectedItem, Me.folder, Me.extension)
        Me.Close()
    End Sub
End Class
