
$.ajaxSetup({contentType:'application/json'});
function login_post(){
	$.post("http://localhost:3000/api/user/login", { json_string:JSON.stringify({name:"jahan", time:"eecs498"}) });
};
