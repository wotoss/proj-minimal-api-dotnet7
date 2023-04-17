# Como criar as databases de test e dev
# Lembrando que tem que rodar direto no diretorio do projeto, principal
# No caso é api => /c/projeto-desenvolvimento-minimal-api/proj_minimal_api_dotnet7/api
```shell
cd api
DATABASE_URL_MINIMAL_API="Data Source=DESKTOP-VUJK6QA\\SQLEXPRESS;Initial Catalog=minimal_api_dotnet7_test;Persist Security Info=True;Integrated Security=SSPI; TrustServerCertificate=True" dotnet ef database update #create base de test

DATABASE_URL_MINIMAL_API="Data Source=DESKTOP-VUJK6QA\\SQLEXPRESS;Initial Catalog=minimal_api_dotnet7;Persist Security Info=True;Integrated Security=SSPI; TrustServerCertificate=True" dotnet ef database update #create base de dev ou prod

#### ou ####
./migration.sh
```

# Rodando test
```shell
DATABASE_URL_MINIMAL_API="Data Source=DESKTOP-VUJK6QA\\SQLEXPRESS;Initial Catalog=minimal_api_dotnet7_test;Persist Security Info=True;Integrated Security=SSPI; TrustServerCertificate=True" dotnet test # compila, sobe servidor, roda test

##### ou #####
./test.sh
```

# Rodando a aplicação
```Shell
cd api
DATABASE_URL_MINIMAL_API="Data Source=DESKTOP-VUJK6QA\\SQLEXPRESS;Initial Catalog=minimal_api_dotnet7;Persist Security Info=True;Integrated Security=SSPI; TrustServerCertificate=True" dotnet run # compila, sobe servidor, roda aplicação

#### ou ####
./run.sh
```

# Dando permissão aos arquivos .sh
```shell
#arquivo run.sh que esta na raiz
chmod +x run.sh

#arquivo test.sh que esta na raiz
chmod +x test.sh
```

# Rodando para ambiente de produção
```shell
./run-prod.sh
```