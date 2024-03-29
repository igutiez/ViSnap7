﻿Imports System.Windows.Forms

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

    Private Sub FilterTextbox_TextChanged(sender As Object, e As EventArgs) Handles FilterTextbox.TextChanged
        RecipeList.Items.Clear()
        LoadAllRecipes(Me, Me.folder, Me.extension)

        Dim items = From it In RecipeList.Items.Cast(Of Object)()
                    Where it.ToString().IndexOf(FilterTextbox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0
        Dim matchingItemList As List(Of Object) = items.ToList()
        RecipeList.BeginUpdate()
        RecipeList.Items.Clear()
        For Each item In matchingItemList
            RecipeList.Items.Add(item)
        Next
        RecipeList.EndUpdate()
    End Sub

    Private Sub RecipeList_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles RecipeList.MouseDoubleClick
        If RecipeList.SelectedIndex >= 0 Then
            LoadRecipe(RecipeList.SelectedItem, Me.folder, Me.extension)
            Me.Close()
        End If
    End Sub
End Class
