import hashlib

import uuid
import os

algorithm = 'sha512' # name of the algorithm to use for

password = 'rebeccapass15' # unencrypted password

salt = '9acb7f5ed8e2414dbfb8a219967d5968' # salt as a hex string for storage in db

m = hashlib.new(algorithm)

m.update(str(salt + password).encode('utf-8'))

password_hash = m.hexdigest()
print "$".join([algorithm,salt,password_hash])

print os.urandom(24)