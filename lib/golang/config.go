package main

type VispCredentials struct{
  client_id string
  client_secret string
  username string
  password string
}

func getCredentials() VispCredentials{
  var creds VispCredentials

  //configure this with your credentials
  creds.client_id = ""
  creds.client_secret = ""
  creds.username = ""
  creds.password = ""

  return creds
}
