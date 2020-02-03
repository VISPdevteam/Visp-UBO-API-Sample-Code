import functions
import json # for turning our dictionaries to json

#get a token for our session. Using "Session" from the requests library, the token is stored on the session "object"
functions.get_token()

data = {
    "operationName": "customer",
    "query": 'query customer($customer_id:Int!){\n customer(customer_id:$customer_id){\n customer_id\n username\n first_name\n last_name\n}\n}',
    "variables": {
        "customer_id": 1432780
    }
};

response = functions.get_ubo_api(data)
# should return a response dictionary
# {
#   'data': {
#       'customer':{
#           'customer_id': <customer id>
#           'username': <username>
#           ...
#       }
#   }
#}
print(response['data']['customer']['customer_id'])
print(response['data']['customer']['username'])

data = response['data']['customer'];
print(data['username'])
