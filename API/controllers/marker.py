from flask import *
from extensions import *
import re
import hashlib
import random

api_marker = Blueprint('api_marker', __name__, template_folder = 'templates')

@api_marker.route('/api/marker/add', methods = ['POST'])
def add_marker_route():
	check_user_and_model(session, request.json)

	cur = db.cursor()
	cur.execute("INSERT INTO Marker (modelid, message, x, y, z) VALUES (%s, %s, %s, %s, %s)",
	 (request.json['modelid'], request.json['message'], request.json['x'], request.json['y'], request.json['z']))
	cur.execute("SELECT LAST_INSERT_ID()");
	result = cur.fetchone();
	return jsonify(markerid = result['LAST_INSERT_ID()'])


@api_marker.route('/api/marker/edit', methods = ['POST'])
def edit_marker_route():
	check_user_and_model(session, request.json)
	check_marker(session, request.json)
	cur = db.cursor()
	cur.execute("UPDATE Marker SET x = %s, y =%s, z = %s, message = %s WHERE markerid = %s", 
		(request.json['x'], request.json['y'], request.json['z'],
		 request.json['message'], request.json['markerid']))
	return jsonify(markerid = request.json['markerid'])

@api_marker.route('/api/marker/delete', methods = ['POST'])
def delete_marker_route():
	if 'user' not in session:
		return jsonify(errors = [{'message': "User not in session"}]), 422
	cur = db.cursor()
	cur.execute("SELECT * FROM Model WHERE modelid = %s AND username = %s", (json['modelid'], session['user']['username']))
	if not cur.fetchone():
		return jsonify(errors = [{'message': "User doesn't have access to this model"}]), 401
	check_marker(session, request.json)

	cur.execute("DELETE FROM Marker WHERE markerid = %s", (request.json['markerid']))
	return jsonify(markerid = request.json['markerid'])


def check_user_and_model(session, json):
	if ('modelid' not in json or 'x' not in json or 'y' not in json or 'z' not in json
		or 'message' not in json):
		return jsonify(errors = [{"message": "missing field"}]), 422
	cur = db.cursor()
	cur.execute("SELECT * FROM Model WHERE modelid = %s AND username = %s", (json['modelid'], session['user']['username']))
	if not cur.fetchone():
		return jsonify(errors = [{'message': "User doesn't have access to this model"}]), 401

def check_marker(session, json):
	if ('markerid' not in json):
		return jsonify(errors = [{"message": "missing field"}]), 422
	cur = db.cursor()
	cur.execute("SELECT * FROM Marker WHERE markerid = %s AND modelid = %s", (json['markerid'], json['modelid']))
	if not cur.fetchone():
		return jsonify(errors = [{'message': "This marker doesn't exist"}]), 401