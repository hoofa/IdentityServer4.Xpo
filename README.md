[The OAuth 2.0 Authorization Framework](https://tools.ietf.org/html/rfc6749)

[OpenID Connect](https://openid.net/connect/)
OIDC：Open ID Connect
OIDC是基于OAuth2的，OAuth2只解决了授权的问题，没有解决认证问题，而OpenID是个认证协议，所以二者结合就是OIDC。
OIDC= OAuth2 + OpenID
OIDC在OAuth2的access_token的基础上增加了身份认证信息， 通过公钥私钥配合校验获取身份等其他信息—– 即idToken

[IdentityServer4](https://identityserver4.readthedocs.io/en/latest/)
Identity Server 4是IdentityServer的最新版本，它是流行的由.NET实现的OpenID Connect和OAuth Framework  

[Devexpresss.XPO](https://documentation.devexpress.com/XPO/7969/Product-Information/Main-Features)
XPO 是 eXpress Persistent Objects的缩写，它是DevExpress公司推出的一个运行在.NETFramwork平台上的ORM工具。Persistent Objects翻译过来时“持久化对象”的意思，所谓的持久化，也就是将数据存储下来，比如存在数据库、文件等这样的形式“永久的”保存下来。XPO是一个ORM工具，它在应用程序代码和数据库之间扮演了一个中间层的角色，起到中间桥梁这样一个作用，简单而言，就是将面向对象编程所建立的对象在数据库中做一个映射，使之和数据库中的表建立一一对应的关系。我们在面向对象编程的时候，只需要关心程序中的“对象”就可以了，XPO会自动的把我们对对象的操作反应到数据库中。



本项目内IdentityServer4.Xpo为IdentityServer4的Xpo仓库实现
IdentityServer 为身份认证服务器
Test.Core为 .net core的 mvc, api 连接IdentityServer身份认证示例
Test.Framework 为 .net Framework的 mvc,api.web form page 连接IdentityServer身份认证示例


License
[Apache License Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html)