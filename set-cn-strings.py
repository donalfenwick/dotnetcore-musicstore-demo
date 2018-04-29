import glob, json, codecs


## CONFIGURATION ##
# Configure your database connection string and then run this script to modify the configuration files for all applications in the solution
sqlServerConnectionString = "Data Source=localhost;Initial Catalog=MusicStoreDemo;User Id=musicstoreuser;Password=mus1cstor£UserPassword;MultipleActiveResultSets=True"
mySqlServerConnectionString = "server=localhost;port=3306;user=musicstoreuser;password=mus1cstor£UserPassword;database=MusicStoreDatabase"
databaseProviderType = "SQLSERVER" ## set to SQLSERVER or MYSQL

## END CONFIGURATION ##


def updateJsonFile(filename):
    jsonFile = open(filename, "r") # Open the JSON file for reading
    data = json.load(codecs.open(filename, 'r', 'utf-8-sig')) # Read the JSON into the buffer
    jsonFile.close() # Close the JSON file

    ## update the 
    if data.get('ConnectionStrings') and data['ConnectionStrings'].get('SqlServerConnection'):
        data["ConnectionStrings"]["SqlServerConnection"] = sqlServerConnectionString
    if data.get('ConnectionStrings') and data['ConnectionStrings'].get('MySqlConnection'):
        data["ConnectionStrings"]["MySqlConnection"] = mySqlServerConnectionString
    if data.get('MusicStoreAppDatabaseProvider'):
        data["MusicStoreAppDatabaseProvider"] = databaseProviderType
    
    ## Save changes to the config file
    jsonFile = open(filename, "w+")
    jsonFile.write(json.dumps(data, indent=4))
    jsonFile.close()


# iterate each config file and update the connection strings
for filename in glob.iglob('./**/appsettings.json', recursive=True):
    print(filename)
    updateJsonFile(filename)
    


