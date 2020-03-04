require 'net/http'
require 'json'
require_relative 'config.rb'

def get_token
  uri = URI(get_token_url())
  https = Net::HTTP.new(uri.host, uri.port)
  https.use_ssl = true

  credentials = get_config()
  request = Net::HTTP::Get.new(uri)
  request['x-visp-client-id'] = credentials["client_id"]
  request['x-visp-client-secret'] = credentials["client_secret"]
  request['x-visp-username'] = credentials["app_username"]
  request['x-visp-password'] = credentials["app_password"]

  response = https.request(request)
  body = response.read_body
  parsedBody = JSON.parse(body)
  token = parsedBody["token"]
  return token
end

def send_request(token, req)
  uri = URI(get_api_url())
  https = Net::HTTP.new(uri.host, uri.port)
  https.use_ssl = true

  if(req.respond_to?(:to_hash)) #convert req JSON string if not yet converted
    req = JSON.generate(req)
  end
  request = Net::HTTP::Post.new(uri)
  request['Content-type'] = 'application/json'
  request['authorization'] = token
  request.body = req

  response = https.request(request)
  body = response.read_body
  data = JSON.parse(body)
  return data
end

def get_url
  return 'https://data.visp.net'
end

def get_token_url
  return get_url().concat('/token')
end

def get_api_url
  return get_url().concat('/graphql')
end
