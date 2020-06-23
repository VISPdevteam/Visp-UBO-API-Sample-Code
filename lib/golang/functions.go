package main

import (
  "strings"
  "net/http"
  "io/ioutil"
  "encoding/json"
)

type UBOApiData struct {
  body string
  method string
  token string
}

func getBaseUrl() string{
  return "https://sandbox.visp.net"
}

func getTokenUrl() string{
  baseUrl := []string{getBaseUrl(), "/token"}
  return strings.Join(baseUrl, "")
}

func getApiUrl() string{
  baseUrl := []string{getBaseUrl(), "/graphql"}
  return strings.Join(baseUrl, "")
}

func getToken() string {
  client := &http.Client{
  }

  credentials := getCredentials();
  req, err := http.NewRequest("GET", getTokenUrl(), nil)
  if err != nil {
    //handle error
  }
  req.Header.Add("x-visp-client-id", credentials.client_id)
  req.Header.Add("x-visp-client-secret", credentials.client_secret)
  req.Header.Add("x-visp-username", credentials.username)
  req.Header.Add("x-visp-password", credentials.password)

  resp, err := client.Do(req)
  defer resp.Body.Close()
  body, err := ioutil.ReadAll(resp.Body)
  if (err != nil) {
    //handle error
  }
  //parse json body
  var result map[string]interface{}
  json.Unmarshal(body, &result)

  return result["token"].(string)
}

func sendApiRequest(data UBOApiData) (map[string]interface{}, interface{}) {
  client := &http.Client{
  }
  payload := strings.NewReader(string(data.body))
  req, err := http.NewRequest(data.method, getApiUrl(), payload)
  if err != nil {
    panic(err);
  }

  req.Header.Add("authorization",data.token)
  req.Header.Add("Content-Type","application/json")

  res, err := client.Do(req)
  defer res.Body.Close()
  body, err := ioutil.ReadAll(res.Body)

  result := make(map[string]interface{})
  json.Unmarshal([]byte(string(body)), &result)

  //return errors if response includes errors
  if errs, ok := result["errors"]; ok {
    return nil, errs
  }

  return result["data"].(map[string]interface{}), nil
}
