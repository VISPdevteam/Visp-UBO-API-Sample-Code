const config = require('./config');
const request = require('request-promise');

//const base_url = 'https://data.visp.net';
const credentials = config.credentials;

const getToken = async () => {
  const headers = {
    'x-visp-client-id': credentials.client_id,
    'x-visp-client-secret': credentials.client_secret,
    'x-visp-username': credentials.username,
    'x-visp-password': credentials.password
  };

  let response;
  try {
    response = await request.get(base_url + '/token',{
      headers,
      json:true,
    });
    response = response.token;
  } catch (e) {
    console.log(e.message);
    response = null;
  }

  return response;
}

const sendRequest = async(data,token) => {
  const headers = {
    'authorization': token,
    'Content-Type': 'application/json'
  };

  let response;
  try {
    response = await request.post(base_url + '/graphql',{
      headers,
      body: data,
      json: true,
    });
  } catch (e) {
    console.log(e.message);
    response = null;
  }

  return response;
}

const isEmptyObject = (obj) => {
    for (let key in obj) {
        if (obj.hasOwnProperty(key)){
            return false;
        }
    }
    return true;
}

module.exports = {
  sendRequest,
  getToken,
  isEmptyObject,
}
