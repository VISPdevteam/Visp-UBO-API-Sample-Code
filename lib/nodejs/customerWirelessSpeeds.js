const functions = require('./functions');

const main = async() => {
  let token = await functions.getToken();

  let data = {
    operationName: 'customer',
    query: 'query customer($customer_id:Int!){\n customer(customer_id:$customer_id){\n package_instances {\n package_id\n package_name\n service_instances{\n service_details {\n ... on ServiceTypeWifi{\n service_number\n username\n up_speed\n down_speed\n up_speed_unit\n down_speed_unit\n}\n}\n}\n}\n}\n}',
    variables: {
      customer_id: 917336
    }
  };

  let response = await functions.sendRequest(data, token);

  //extract data
  if (response.errors && response.errors.length){
    console.log(response.errors);
  } else {
    let data = response.data;
    let customer = data.customer;
    customer.package_instances.forEach(package_instance => {
        package_instance.service_instances.forEach(service_instance => {
            let wifiService = service_instance.service_details;
            if (!functions.isEmptyObject(wifiService)) {
                console.log('Service Username: '+wifiService.username);
                console.log('Down speed: '+wifiService.down_speed+' '+wifiService.down_speed_unit);
                console.log('Up speed: '+wifiService.up_speed+' '+wifiService.up_speed_unit);
            }
        })
    })
  }
}

main();
