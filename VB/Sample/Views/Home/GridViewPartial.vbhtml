@ModelType List(Of Sample.Models.User)
@Html.DevExpress().GridView( _
    Sub(settings)
            settings.Name = "gridView"
            settings.KeyFieldName = "ID"
            settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "GridViewPartial"}
            settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow
            settings.SettingsEditing.AddNewRowRouteValues = New With {.Controller = "Home", .Action = "AddNew"}
            settings.SettingsEditing.UpdateRowRouteValues = New With {.Controller = "Home", .Action = "Update"}
            settings.SettingsEditing.DeleteRowRouteValues = New With {.Controller = "Home", .Action = "Delete"}
            settings.CommandColumn.Visible = True
            settings.CommandColumn.NewButton.Visible = True
            settings.CommandColumn.DeleteButton.Visible = True
            settings.CommandColumn.EditButton.Visible = True
            settings.Columns.Add("NickName")
            settings.Columns.Add( _
                Sub(column)
                        column.Caption = "Avatar"
                        column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center
                        column.SetDataItemTemplateContent( _
                            Sub(c)
                                    Dim _url As String = DirectCast(DataBinder.Eval(c.DataItem, "AvatarUrl"), String)
                                    ViewContext.Writer.Write(If((Not String.IsNullOrEmpty(_url)), "<img src=""" & Url.Content(_url) & """/>", "<center>Click Edit to load a New Avatar</center>"))
                            End Sub)
                End Sub)
            settings.SetEditFormTemplateContent( _
                Sub(c)
                        Dim user = If(ViewData("UserInfo") IsNot Nothing, ViewData("UserInfo"), c.DataItem)
                        Session("UserID") = If(DataBinder.Eval(user, "ID") IsNot Nothing, DataBinder.Eval(user, "ID"), Model.Count + 1)
                        ViewContext.Writer.Write("<div style=""padding: 4px 0px; font-size: 8pt"">")
                        Html.DevExpress().Label( _
                            Sub(edtSettings)
                                    edtSettings.Text = "NickName"
                                    edtSettings.AssociatedControlName = "NickName"
                                    edtSettings.ControlStyle.Font.Bold = True
                            End Sub).Render()
                        Html.DevExpress().TextBox( _
                            Sub(edtSettings)
                                    edtSettings.Name = "NickName"
                                    edtSettings.ShowModelErrors = True
                                    edtSettings.Width = System.Web.UI.WebControls.Unit.Pixel(200)
                            End Sub).Bind(DataBinder.Eval(user, "NickName")).Render()
                        ViewContext.Writer.Write("</div>")
                        ViewContext.Writer.Write("<div style=""padding: 4px 0px; font-size: 8pt"">")
                        Html.DevExpress().Label( _
                            Sub(edtSettings)
                                    edtSettings.Text = "Avatar"
                                    edtSettings.ControlStyle.Font.Bold = True
                            End Sub).Render()
                        ViewContext.Writer.Write("<br />")
                        ViewContext.Writer.Write("<div style=""margin-top: 2px"">" + "Allowed ContentTypes: image/jpeg<br />" + "Maximum File Size: 4Mb" + "</div>")
                        ViewContext.Writer.Write("<br />")
                        Using Html.BeginForm("ImageUpload", "Home", FormMethod.Post)
                            Html.DevExpress().UploadControl( _
                                Sub(ucSettings)
                                        ucSettings.Name = "uploadControl"
                                        ucSettings.ShowUploadButton = True
                                        ucSettings.AddUploadButtonsSpacing = 0
                                        ucSettings.AddUploadButtonsHorizontalPosition = AddUploadButtonsHorizontalPosition.InputRightSide
                                        ucSettings.CallbackRouteValues = New With {.Controller = "Home", .Action = "ImageUpload"}
                                        ucSettings.ValidationSettings.Assign(Sample.Controllers.UploadControlHelper.ValidationSettings)
                                        ucSettings.ClientSideEvents.FileUploadComplete = "function(s, e) { if(e.isValid) { avatarUrl.SetValue(e.callbackData) } }"
                                End Sub).Render()
                        End Using
                        Html.DevExpress().Label( _
                           Sub(edtSettings)
                                   edtSettings.Text = "Avatar Url:"
                                   edtSettings.AssociatedControlName = "avatarUrl"
                           End Sub).Render()
                        Html.DevExpress().TextBox( _
                           Sub(edtSettings)
                                   edtSettings.Name = "avatarUrl"
                                   edtSettings.Width = System.Web.UI.WebControls.Unit.Percentage(100)
                           End Sub).Bind(DataBinder.Eval(user, "AvatarUrl")).Render()
                        ViewContext.Writer.Write("</div>")
                        ViewContext.Writer.Write("<div style=""text-align: right; padding: 2px 2px 2px 2px"">" + String.Format("<a href=""#"" onclick=""{0}.UpdateEdit()"">Update</a>&nbsp;", settings.Name) + String.Format("<a href=""#"" onclick=""{0}.CancelEdit()"">Cancel</a>", settings.Name) + "</div>")
                End Sub)
    End Sub).Bind(Model).GetHtml()