from flask import Flask, render_template
import extensions
import controllers
import config
import http
http.__file__

# Initialize Flask app with the template folder address
app = Flask(__name__, template_folder='templates')
app.secret_key = 'A0Zr98j/3yX R~XHH!jmDS2\12WX/,?RT'

# Register the controllers
app.register_blueprint(controllers.api_user)
app.register_blueprint(controllers.api_marker)
app.register_blueprint(controllers.api_model)
app.register_blueprint(controllers.api_invite)

# Listen on external IPs
# For us, listen to port 3000 so you can just run 'python app.py' to start the server
if __name__ == '__main__':
    # listen on external IPs
    app.run(host=config.env['host'], port=config.env['port'], debug=True)
