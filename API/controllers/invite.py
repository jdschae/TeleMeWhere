from flask import *
from extensions import *
import re
import hashlib

api_invite = Blueprint('api_invite', __name__, template_folder = 'templates')

@api_invite.route('/api/invite/add', methods = ['POST'])
def add_invite_route():
	if 'username' not in request.json or 'invitee' not in request.json:
		return jsonify(errors = [{"message": "missing field"}]), 422
	cur = db.cursor()
	if request.json['username'] == request.json['invitee']:
		return jsonify(errors = [{"message": "username is the same as invitee"}]), 422
	cur.execute("SELECT * FROM User WHERE username = %s", request.json['username'])
	if not cur.fetchone():
		return jsonify(errors = [{"message": "User doesn't exist"}]), 422
	cur.execute("SELECT * FROM User WHERE username = %s", request.json['invitee'])
	if not cur.fetchone():
		return jsonify(errors = [{"message": "Invitee doesn't exist"}]), 422
	cur.execute("INSERT INTO Invite (username, invitee, access) VALUES (%s, %s, 0)",
	 (request.json['username'], request.json['invitee']))
	return jsonify(invitee = request.json['invitee'])

@api_invite.route('/api/invite/accept', methods = ['POST'])
def accept_invite_route():
	if 'username' not in request.json or 'invitee' not in request.json:
		return jsonify(errors = [{"message": "missing field"}]), 422
	cur = db.cursor()
	if request.json['username'] == request.json['invitee']:
		return jsonify(errors = [{"message": "username is the same as invitee"}]), 422
	cur.execute("SELECT * FROM User WHERE username = %s", request.json['username'])
	if not cur.fetchone():
		return jsonify(errors = [{"message": "invitee doesn't exist"}]), 422
	cur.execute("UPDATE Invite SET access = 1 WHERE username = %s AND invitee = %s",
	 (request.json['username'], request.json['invitee']))
	return jsonify(invitee = request.json['invitee'])

@api_invite.route('/api/invite/view', methods = ['POST'])
def view_invite_route():
	if 'username' not in request.json:
		return jsonify(errors = [{"message": "missing field"}]), 422
	cur = db.cursor()
	cur.execute("SELECT * FROM Invite WHERE invitee = %s", request.json['username'])
	results = cur.fetchall()
	inviters = ""
	for result in results:
		inviters = inviters + result['username'] + ";"
	return inviters;

@api_invite.route('/api/invite/delete', methods = ['POST'])
def delete_invite_route():
	if 'username' not in request.json or 'invitee' not in request.json:
		return jsonify(errors = [{"message": "missing field"}]), 422
	cur = db.cursor()
	if request.json['username'] == request.json['invitee']:
		return jsonify(errors = [{"message": "username is the same as invitee"}]), 422
	cur.execute("SELECT * FROM User WHERE username = %s", request.json['username'])
	if not cur.fetchone():
		return jsonify(errors = [{"message": "invitee doesn't exist"}]), 422
	cur.execute("DELETE FROM Invite WHERE username = %s AND invitee = %s",
	 (request.json['username'], request.json['invitee']))
	return jsonify(invitee = request.json['invitee'])

