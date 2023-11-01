# Blog_WithIdentity

скрытые поля для DiscussionPage

@Html.HiddenFor(model => model.Id)
@Html.HiddenFor(model => model.Title)
@Html.HiddenFor(model => model.Text)
@Html.HiddenFor(model => model.PublicationDate)
@Html.HiddenFor(model => model.UserId)
@Html.HiddenFor(model => model.User)
@foreach (var comment in Model.Comments){
@Html.HiddenFor(model => comment)
}
@foreach (var teg in Model.Tegs){
@Html.HiddenFor(model => teg)
}
