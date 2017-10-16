from flask import *
from extensions import *
import re
import hashlib

api_user = Blueprint('api_user', __name__, template_folder = 'templates')


@api_user.route('/api/user/login',methods = ['POST'])
def login_route():
	if 'user' in session:
		return jsonify(errors = [{'message': "Please log out first"}]), 422
	print (request.get_json())
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
	print("good")
	return jsonify(username = result['username'],
				   firstname = result['firstname'],
				   lastname = result['lastname'])


@api_user.route('/api/user/logout', methods = ['POST'])
def logout_route():
	if 'user' in session:
		session.pop('user')
		return (jsonify(''), 204)
	else:
		return jsonify(errors = [{'message': "You do not have the "
					   "necessary credentials for the resource"}]), 401

@api_user.route('/api/user/create', methods = ['POST'])
def create_route():
	request_json = request.get_json()
	if ('username' not in request_json or 'firstname' not in request_json
		or 'lastname' not in request_json or 'password' not in request_json):
		return jsonify(errors = [{"message": "missing field"}]), 422
	else:
		pw_hash = encrypt_password(request.json['password'])
		cur = db.cursor()
		cur.execute("SELECT * FROM User Where username = %s", request_json['username'])
		if (cur.fetchone()):
			return jsonify(errors = [{"message": "username already exists"}]), 422
		cur.execute("INSERT INTO User (username, firstname, lastname, password) "
						"VALUES(%s, %s, %s, %s)", (request_json['username'],
						request_json['firstname'], request_json['lastname'],
						pw_hash))
	return jsonify(success = [{'message': "User created"}]), 200



def encrypt_password(password):
	m = hashlib.new("sha512")
	salt = "498"
	m.update(str(salt + password).encode('utf-8'))
	pw_hash = m.hexdigest()
	return pw_hash

