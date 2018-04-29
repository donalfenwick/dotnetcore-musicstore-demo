## CONFIGURATION ##
# Configure your database connection string and then run this script to modify the configuration files for all applications in the solution
$sqlServerConnectionString = "Data Source=localhost;Initial Catalog=MusicStoreDemo;User Id=musicstoreuser;Password=mus1cstor£UserPassword;MultipleActiveResultSets=True"
$mySqlServerConnectionString = "server=localhost;port=3306;user=musicstoreuser;password=mus1cstor£UserPassword;database=MusicStoreDatabase"
$databaseProviderType = "SQLSERVER" ## set to SQLSERVER or MYSQL

## END CONFIGURATION ##



Clear-Host

# inlcude fubction to format the json back to standard spacing after the rewrite
# credit https://www.bountysource.com/issues/39427104-use-prettier-formatting-for-convertto-json
function Format-Json([Parameter(Mandatory, ValueFromPipeline)][String] $json) {
  $indent = 0;
  ($json -Split '\n' |
    % {
      if ($_ -match '[\}\]]') {
        # This line contains  ] or }, decrement the indentation level
        $indent--
      }
      $line = (' ' * $indent * 2) + $_.TrimStart().Replace(':  ', ': ')
      if ($_ -match '[\{\[]') {
        # This line contains [ or {, increment the indentation level
        $indent++
      }
      $line
  }) -Join "`n"
}

Get-ChildItem -Path appsettings.json -Recurse -Force | ForEach-Object { 

    # Combines source folder path and file name
    $configfile = $_;
    Write-Host "Process file $configfile";
    $json = Get-Content $configfile | Out-String | ConvertFrom-Json

    if(
        ($json.ConnectionStrings -and $json.ConnectionStrings.SqlServerConnection) -or 
        ($json.ConnectionStrings -and $json.ConnectionStrings.MySqlConnection) -or 
        $json.MusicStoreAppDatabaseProvider
    ){
        if($json.ConnectionStrings -and $json.ConnectionStrings.SqlServerConnection){
            $json.ConnectionStrings.SqlServerConnection = $sqlServerConnectionString;
        }
        if($json.ConnectionStrings -and $json.ConnectionStrings.MySqlConnection){
            $json.ConnectionStrings.MySqlConnection = $mySqlServerConnectionString;
        }
        if($json.MusicStoreAppDatabaseProvider){
            $json.MusicStoreAppDatabaseProvider = $databaseProviderType;
        }
        # write json data back out to the config file
        $json | ConvertTo-Json | Format-Json | set-content $configfile
    }
    

}

