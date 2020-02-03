#this sample uses the requests library for python.
#for more info on this library, visit https://requests.readthedocs.io/en/master/
import requests
import json
import config

session = requests.Session()
api_url = "https://data.visp.net"
route = "/graphql"

def send_request(method, url, headers={},data={}):
    if method == 'POST':
        r = session.post(url,headers=headers, data=data)
    if method == 'GET':
        r = session.get(url, headers=headers, params=data)
    #the other request types will not be necessary since get and post are mostly the ones used
    #in communicating with the API
    return r

def get_ubo_api(data):
    headers = {
        "Content-Type": "application/json"
    }
    data = json.dumps(data) #convert our dictionary to json string

    response = send_request('POST', api_url+route, headers, data)
    if (response.status_code == 200):
        #the api will, most of the time, return a json object.
        return response.json()
    else:
        #output the response errors
        print(response.status_code)
        print(response.text)

def get_token():
    headers = {
        "x-visp-client-id": config.client_id,
        "x-visp-client-secret": config.client_secret,
        "x-visp-username": config.app_username,
        "x-visp-password": config.app_password
    }
    response = requests.get(api_url + "/token",headers=headers)
    response = response.json()
    token = response['token']
    #set on our session object header so it will be sent on every request
    session.headers.update({"authorization":token})

    return token;
