для Index страницы:
```cshtml
@using Microsoft.AspNet.Identity.EntityFramework;

@{
  var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
  var currentUser = userManager.FindById(User.Identity.GetUserId());
}
@if (currentUser != null)
{
  <div>User exists!</div>
}
else
{
  <div>User does not exist.</div>
}
```
