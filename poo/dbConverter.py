#this code converts the sql made in xampp to an actual database...
import sqlite3
sqlPath = "library.sql"
dbPath = "library.db"
with open(sqlPath, 'r', encoding='utf-8') as f:
    sqlS = f.read()
conn = sqlite3.connect(dbPath)
cursor = conn.cursor()
try:
    cursor.executescript(sqlS)
    print(f"Database '{dbPath}' created from '{sqlPath}'")
except Exception as e:
    print("Error:", e)
conn.commit()
conn.close()

#why cant i just use ittttt aaaaa databases are such a pain in my ass