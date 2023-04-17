
cd api
echo "====[Rodando migração prod]===="
DATABASE_URL_MINIMAL_API="Data Source=DESKTOP-VUJK6QA\\SQLEXPRESS;Initial Catalog=minimal_api_dotnet7_prod;Persist Security Info=True;Integrated Security=SSPI; TrustServerCertificate=True" dotnet ef database update #create base de dev ou prod

dotnet publish -o Reliase #definindo a pasta de publicação
DATABASE_URL_MINIMAL_API="Data Source=DESKTOP-VUJK6QA\\SQLEXPRESS;Initial Catalog=minimal_api_dotnet7_prod;Persist Security Info=True;Integrated Security=SSPI; TrustServerCertificate=True" dotnet ef database update #create base de dev ou prod
dotnet Release/proj_minimal_api_dotnet7.dll

#subi a aplicação em produção
#dotnet Release/proj_minimal_api_dotnet7.dll