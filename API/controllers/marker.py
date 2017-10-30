from flask import *
from extensions import *
import re
import hashlib
import random

api_marker = Blueprint('api_marker', __name__, template_folder = 'templates')

@api_marker.route('/api/marker/add', methods = ['POST'])
def add_marker_route():
	if ( 'x' not in request.json or 'y' not in request.json or 'z' not in request.json
		or 'message' not in request.json or 'username' not in request.json):
		return jsonify(errors = [{"message": "missing field"}]), 422
	if (request.json['username'] == ""):
		return jsonify(errors = [{"message": "not logged in"}]), 422
	cur = db.cursor()
	cur.execute("SELECT * FROM Model WHERE username = %s", request.json['username'])
	modelid = cur.fetchone()['modelid']
	cur.execute("INSERT INTO Marker (modelid, message, x, y, z) VALUES (%s, %s, %s, %s, %s)",
	 (modelid, request.json['message'], request.json['x'], request.json['y'], request.json['z']))
	cur.execute("SELECT LAST_INSERT_ID()");
	result = cur.fetchone();
	return str(result['LAST_INSERT_ID()'])


@api_marker.route('/api/marker/edit', methods = ['POST'])
def edit_marker_route():
	'''
	if 'user' not in session:
		return jsonify(errors = [{'message': "User not in session"}]), 422
	'''
	if ( 'x' not in request.json or 'y' not in request.json or 'z' not in request.json
		or 'message' not in request.json or 'username' not in request.json):
		return jsonify(errors = [{"message": "missing field"}]), 422
	if (request.json['username'] == ""):
		return jsonify(errors = [{"message": "not logged in"}]), 422
	if ('markerid' not in request.json):
		return jsonify(errors = [{"message": "missing field"}]), 422
	cur = db.cursor()
	cur.execute("SELECT * FROM Model WHERE username = %s", request.json['username'])
	modelid = cur.fetchone()['modelid']
	cur.execute("SELECT * FROM Marker WHERE markerid = %s AND modelid = %s", (request.json['markerid'], modelid))
	if not cur.fetchone():
		return jsonify(errors = [{'message': "This marker doesn't exist"}]), 401

	cur = db.cursor()
	cur.execute("UPDATE Marker SET x = %s, y =%s, z = %s, message = %s WHERE markerid = %s", 
		(request.json['x'], request.json['y'], request.json['z'],
		 request.json['message'], request.json['markerid']))
	return jsonify(markerid = request.json['markerid'])

@api_marker.route('/api/marker/delete', methods = ['POST'])
def delete_marker_route():
	'''
	if 'user' not in session:
		return jsonify(errors = [{'message': "User not in session"}]), 422
	'''
	cur = db.cursor()
	if ('markerid' not in request.json or 'username' not in request.json):
		return jsonify(errors = [{"message": "missing field"}]), 422
	if (request.json['username'] == ""):
		return jsonify(errors = [{"message": "not logged in"}]), 422
	cur.execute("SELECT * FROM Model WHERE username = %s", request.json['username'])
	modelid = cur.fetchone()['modelid']
	print (modelid + " ： " + request.json['markerid'])
	cur.execute("DELETE FROM Marker WHERE  markerid = %s AND modelid = %s", (request.json['markerid'], modelid))

	return jsonify(modelid = modelid)

