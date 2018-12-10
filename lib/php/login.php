<?php

// Sample code on how to generate token using your VISP credentials.

require 'functions.php';

$clientId = ""; // VISP API client ID
$clientSecret = ""; // VISP API client secret
$username = ""; // VISP UBO appuser username
$password = ""; // VISP UBO appuser password


$UBOOAuth = getUboAuth($clientId, $clientSecret, $username, $password);
$accessToken = $UBOOAuth->getAccessUsingPassword();
echo $accessToken->token;
