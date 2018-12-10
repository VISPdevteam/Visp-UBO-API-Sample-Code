<?php

$base_url = 'https://data.visp.net';
$route = '/graphql';


class UBOOAuth {
    protected $url;
    protected $client_id;
    protected $client_secret;
    protected $token;
    protected $username;
    protected $password;

    public function __construct($client_id, $client_secret) {
        $this->setClient($client_id,$client_secret);
    }

    public function setToken($token) {
        $this->token = $token;
    }

    function getToken() {
        return $this->token;
    }

    function setCredentials($username, $password=''){
        if (gettype($username) == 'array' && empty($password)){
            $this->username = $username['username'];
            $this->password = $username['password'];
        }
        $this->username = $username;
        $this->password = $password;
    }
    
    function setClient($client_id, $client_secret=''){
        if (gettype($client_id) == 'array' && empty($client_secret)){
            $this->client_id = $client_id['client_id'];
            $this->client_secret = $client_id['client_secret'];
        }
        $this->client_id = $client_id;
        $this->client_secret = $client_secret;
    }

    function setUrl($url) {
        $this->url = $url;
    }

    public function getUrl() {
        return $this->url;
    }

    public function getTokenHeader() {
        return array("x-visp-client-id: $this->client_id",
            "x-visp-client-secret: $this->client_secret",
            "x-visp-username: $this->username",
            "x-visp-password: $this->password");
    }

    public function getHeader() {
        $token = $this->token;
        return array("Content-Type: application/json", "authorization: $token");
    }
    
    function getClient() {
        return [
            'client_id' => $this->client_id,
            'client_secret' => $this->client_secret,
        ];
    }
    
    function getCredentials() {
        return [
            'username' => $this->username,
            'password' => $this->password,
        ];
    }

    public function getAccessUsingPassword() {
        return curlAPI($this->url, null, $this->getTokenHeader(), "GET");
    }
}

class UBOApi {
    protected $UBOOAuth;
    protected $url;
    protected $data;
    protected $request_type;
    
    public function __construct($args) {
        $this->setData($args['data']);
        $this->setUBOOAuth($args['auth']);
        $this->setUrl($args['url']);
        $this->setRequestType($args['request_type']);
    }

    function setUBOOAuth($UBOOAuth) {
        $this->UBOOAuth = $UBOOAuth;
    }

    function setUrl($url) {
        $this->url = $url;
    }

    function setData($data) {
        $this->data = $data;
    }

    function setRequestType($request_type) {
        $this->request_type = $request_type;
    }

    public function execute($timeout=DEFAULT_TIMEOUT) {
        $uboAuth = $this->UBOOAuth;
        
        if (is_null($uboAuth)) {
            throw new Exception('UBOOAuth is missing');
        }

        if (!is_null($uboAuth->getToken())) {
            $json_response = curlAPI($this->url, $this->data, $uboAuth->getHeader(), $this->request_type, $timeout);
        } else {
            throw new Exception($json_response->error);
        }

        return $json_response;
    }
}


function curlAPI($url, $data, $header, $request_type = "POST", $timeout=30) {
    $curl = curl_init($url);
    curl_setopt($curl, CURLOPT_HTTPHEADER, $header);
    if ($request_type == "POST") {
        curl_setopt($curl, CURLOPT_POST, 1);
        curl_setopt($curl, CURLOPT_POSTFIELDS, $data);
    } else {
        curl_setopt($curl, CURLOPT_CUSTOMREQUEST, $request_type);
        if ($request_type == "GET") {
            curl_setopt($curl, CURLOPT_HTTPGET, 1);
            curl_setopt($curl, CURLOPT_POST, 0);
        } else if ($request_type == "PUT" || $request_type == 'DELETE') {
            curl_setopt($curl, CURLOPT_POSTFIELDS, $data);
        } 
    }

    curl_setopt($curl, CURLOPT_VERBOSE, 1);
    curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($curl, CURLOPT_SSL_VERIFYHOST, 0);
    curl_setopt($curl, CURLOPT_SSL_VERIFYPEER, false);
    curl_setopt($curl, CURLOPT_TIMEOUT, $timeout);

    if (strpos($url, 'https') !== false) {
        curl_setopt($curl, CURLOPT_SSLVERSION, CURL_SSLVERSION_TLSv1);
    } else {
        curl_setopt($curl, CURLOPT_SSLVERSION, 1);
    }

    $json_response = curl_exec($curl);

    if (false === $json_response) {
        $error = new stdClass();
        $error->message = curl_error($curl);
        $error->error = curl_errno($curl);
        $error->url = $url;
        $error->data = json_decode($data);
        $error->header = $header;
        $error->request_type = $request_type;
        $error->response = json_decode($json_response);
        curl_close($curl);
        return json_decode(json_encode($error));
    }

    curl_close($curl);
    return json_decode($json_response);
}


function getUBOApi($data, $timeout=DEFAULT_TIMEOUT){
    global $route, $base_url;
    $body = $data['body'];
    $requestType = $data['request_type'];

    $url = isset($data['base_url'])?$data['base_url']:$base_url;
    $path = isset($data['route'])?$data['route']:$route;

    $clientId = get_option('graphql_client_id');
    $clientSecret = get_option('graphql_client_secret');
    $username = get_option('graphql_client_user_name',true);
    $password = get_option('graphql_client_pass',true);

    $clientId = isset($data['graphql_client_id'])?$data['graphql_client_id']:$clientId;
    $clientSecret = isset($data['graphql_client_secret'])?$data['graphql_client_secret']:$clientSecret;
    $username = isset($data['client_user_name'])?$data['client_user_name']:$username;
    $password = isset($data['client_password'])?$data['client_password']:$password;

    $UBOOAuth = getUboAuth($clientId, $clientSecret, $username, $password);
    $accessToken = $UBOOAuth->getAccessUsingPassword();
    $UBOOAuth->setToken($accessToken->token);

    $UBOApi = new UBOApi([
        'data' => $body,
        'auth' => $UBOOAuth,
        'url' => $url.$path,
        'request_type' => $requestType,
    ]);
    
    $json_response = $UBOApi->execute($timeout);

    $json_encoded = json_encode($json_response); 
    return $json_encoded;
}

function getUboAuth($clientId, $clientSecret, $username, $password){
    global $base_url;
    $auth = new UBOOAuth($clientId, $clientSecret); 
    $auth->setCredentials($username,$password);
    $auth->setUrl($base_url."/token");
    return $auth;
}
