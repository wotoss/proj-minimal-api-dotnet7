cd api
echo "====[Rodando migração test]===="
DATABASE_URL_MINIMAL_API="Data Source=DESKTOP-VUJK6QA\\SQLEXPRESS;Initial Catalog=minimal_api_dotnet7_test;Persist Security Info=True;Integrated Security=SSPI; TrustServerCertificate=True" dotnet ef database update #create base de test
echo "====[Rodando migração dev]===="
DATABASE_URL_MINIMAL_API="Data Source=DESKTOP-VUJK6QA\\SQLEXPRESS;Initial Catalog=minimal_api_dotnet7;Persist Security Info=True;Integrated Security=SSPI; TrustServerCertificate=True" dotnet ef database update #create base de dev ou prod
