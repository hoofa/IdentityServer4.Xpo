need run IdentityServer first
vist http://localhost:5003/api/token/test/test get api token  and http://localhost:5003/api/token/your_refesh_token to refresh new token

Call api http://localhost:5003/api/apitest need add http head "Authorization:Bearer your_token"
