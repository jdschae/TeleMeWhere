from flask import *
from extensions import *
import re
import hashlib

api_user = Blueprint('api_user', __name__, template_folder = 'templates')


@api_user.route('/api/user/login',methods = ['POST'])
def login_route():
	if 'username' not in request.json or 'password' not in request.json:
		return jsonify(errors = [{'message': "You did not provide the "
					   "necessary fields"}]), 422
	cur = db.cursor()
	cur.execute("SELECT * FROM User WHERE username = %s", (request.json['username']))
	result = cur.fetchone()
	if not result:
		return jsonify(errors = [{'message': "Username does not exist"}]), 404
	if (encrypt_password(request.json['password']) != result['password']):
		return jsonify(errors = [{'message': "Password is incorrect for "
					   "the specified username"}]), 422
	session['user'] = {
		'username': result['username'],
		'firstname': result['firstname'],
		'lastname': result['lastname']
	}
	return jsonify(username = result['username'])


@api_user.route('/api/user/logout', methods = ['POST'])
def logout_route():
	if 'username' not in request.json:
		return jsonify(errors = [{'message': "You did not provide the "
					   "necessary fields"}]), 422
	if (request.json['username'] == ""):
		return jsonify(errors = [{"message": "not logged in"}]), 422
	return jsonify(username = "")

@api_user.route('/api/user/create', methods = ['POST'])
def create_route():
	request_json = request.get_json()
	if ('username' not in request_json or 'firstname' not in request_json
		or 'lastname' not in request_json or 'email' not in request_json
		or 'password' not in request_json or 'sex' not in request_json
		or 'type' not in request_json):
		return jsonify(errors = [{"message": "missing field"}]), 422
	else:
		pw_hash = encrypt_password(request.json['password'])
		cur = db.cursor()
		cur.execute("SELECT * FROM User Where username = %s", request_json['username'])
		if (cur.fetchone()):
			return jsonify(errors = [{"message": "username already exists"}]), 422
		cur.execute("INSERT INTO User (username, firstname, lastname, password, email, sex, type) "
						"VALUES(%s, %s, %s, %s, %s, %s, %s)", (request_json['username'],
						request_json['firstname'], request_json['lastname'],
						pw_hash, request_json['email'], request_json['sex'],
						request_json['type']))
		cur.execute("INSERT INTO Model (username) VALUES (%s)", request.json['username'])
	return jsonify(username = request_json['username'])


@api_user.route('/api/user/info', methods = ['POST'])
def view_user_info_route():
	if (request.json['username'] == ""):
		return jsonify(errors = [{"message": "not logged in"}]), 422
	cur = db.cursor()
	cur.execute("SELECT * FROM User WHERE username = %s;", request.json['username'])
	result = cur.fetchone()
	return result['sex'] + ";" + result["type"]

@api_user.route('/api/user/edit', methods = ['POST'])
def edit_user_info_route():
	request_json = request.get_json()
	if ('username' not in request_json or 'firstname' not in request_json
		or 'lastname' not in request_json or 'email' not in request_json
		or 'password' not in request_json):
		return jsonify(errors = [{"message": "not enough info"}]), 422
	cur = db.cursor()
	for info in request_json:
		if (request_json[info] != "" and info != "username"):
			sql = "UPDATE User SET " + info + " = \"" + request_json[info] + "\" WHERE username = \"" + request_json['username'] + "\""
			cur.execute(sql)
	return "success"


def encrypt_password(password):
	m = hashlib.new("sha512")
	salt = "498"
	m.update(str(salt + password).encode('utf-8'))
	pw_hash = m.hexdigest()
	return pw_hash

