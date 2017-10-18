from flask import *
from extensions import *
import re
import hashlib
import random

api_model = Blueprint('api_model', __name__, template_folder = 'templates')

@api_model.route('/api/model/add', methods = ['POST'])
def add_model_route():
	if 'user' not in session:
		return jsonify(errors = [{'message': "User not in session"}]), 422
	cur = db.cursor()
	cur.execute("INSERT INTO Model (username) VALUES (%s)", (session['user']['username']))
	cur.execute("SELECT LAST_INSERT_ID()");
	result = cur.fetchone();
	return jsonify(modelid = result['LAST_INSERT_ID()'])

@api_model.route('/api/model/view', methods = ['GET'])
def view_model_route():
	if 'user' not in session:
		return jsonify(errors = [{'message': "Please log in first"}]), 401
	cur = db.cursor()
	cur.execute("SELECT * FROM Model WHERE username = %s;", (session['user']['username']))
	modelid = cur.fetchone()['modelid']
	cur.execute("SELECT * FROM Marker WHERE modelid = %s", modelid)
	results = cur.fetchall()
	if results:
		return jsonify(markers = results)
	else:
		return jsonify(errors = [{'message': "You do not have the "
					   "necessary credentials for the resource"}]), 401

@api_model.route('/api/model/delete', methods = ['POST'])
def delete_model_route():
	if 'user' not in session:
		return jsonify(errors = [{'message': "User not in session"}]), 422
	if 'modelid' not in request.json:
		return jsonify(errors = [{'message': "You did not provide the "
					   "necessary fields"}]), 422
	cur = db.cursor() 
	cur.execute("SELECT * FROM Model WHERE username = %s", (request.json['modelid'], session['user']['username']))
	if not cur.fetchone():
		return jsonify(errors = [{'message': "User doesn't have this model"}]), 401
	cur.execute("DELETE FROM Model WHERE modelid = %s", request.json['modelid'])
	return jsonify(message = [{'message': "model deleted"}]), 200

@api_model.route('/api/model/edit', methods = ['POST'])
def edit_model_route():
	print ("TBD")
				   	   