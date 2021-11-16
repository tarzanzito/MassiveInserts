#!/usr/bin/python

import sys
import os
import sqlite3
import uuid
import string
import random
import time
 
date_time_format = "%Y/%m/%d"
 
def create_file(full_file_name):
    """ create file """
 
    if os.path.exists(full_file_name):
        return
 
    path_parts = full_file_name.split(os.sep) 
 
    init = 0
    end = len(path_parts) - 1
    path = ""
 
    if ":" in path_parts[0]: 
        init += 1
        path = path_parts[0] + os.sep
 
    for item in range(init, end):
        path = os.path.join(path, path_parts[item])
        if not os.path.exists(path):
            os.makedirs(path)
 
    open(full_file_name, 'w+')
 
def create_connection(db_file):
    """ create a database connection to a SQLite database """
    
    conn = None
    try:
        conn = sqlite3.connect(db_file)
        print(sqlite3.version)
    except sqlite3.Error as e:
        print(e)
        raise SystemExit
        sys.exit(1)

    return conn
 
def create_table(conn):
    """ create a table from the create_table_sql statement
    :param conn: Connection object
    :return:
    """
 
    sql = """CREATE TABLE IF NOT EXISTS products (
        id integer PRIMARY KEY AUTOINCREMENT,
        guid text NOT NULL, 
        name text NOT NULL,
        price integer NOT NULL,
        validation text NOT NULL
        );"""
 
    try:
        conn.execute(sql)  
        #ccursor = conn.cursor()
        #ccursor.execute(sql)
        conn.commit()
        
    except sqlite3.Error as e:
        print(e)
        raise SystemExit
        sys.exit(1)

def insert_record(conn, guid, name, price, validation):
    """ create a taaaaaaaaaaaaaaaaaaaaaaaable from the create_table_sql statement
    :param conn: Connection object
    :return:
    """
    
    sql = "INSERT INTO products VALUES (NULL, '" + guid + "', '" + name + "', '" + str(price) + "', '" + validation + "');"
 
    try:
        conn.execute(sql)  
        #ccursor = conn.cursor()
        #ccursor.execute(sql)
        conn.commit()
    except sqlite3.Error as e:
        print(e)
        raise SystemExit
        sys.exit(1)
        
def insert_records(conn):
    """ create a taaaaaaaaaaaaaaaaaaaaaaaable from the create_table_sql statement
    :param conn: Connection object
    :return:
    """
 
    for x in range(9999999):
        guid = str(uuid.uuid4())
        name = ""
        for _ in range(255):
            name += random.choice(string.ascii_uppercase + string.digits)
        #name = ''.join(random.choice(string.ascii_uppercase + string.digits) for _ in range(255)) #não gosto nada desta linha
        price = random.randint(1, 100000)
        validation = generate_date_time()
        insert_record(conn, guid, name, price, validation)
        print(x)
 
def generate_date_time():
    """Get a time at a proportion of a range of two formatted times.
 
    start and end should be strings specifying times formated in the
    given format (strftime-style), giving an interval [start, end].
    prop specifies how a proportion of the interval to be taken after
    start.  The returned time will be in the specified format.
    """
    
    #date_time_format= "%Y/%m/%d"
    prop = random.random()
 
    stime = time.mktime(time.strptime("2000/01/01", date_time_format))
    etime = time.mktime(time.strptime("2050/12/31", date_time_format))
 
    ptime = stime + prop * (etime - stime)
 
    return time.strftime(date_time_format, time.localtime(ptime))
 
if __name__ == '__main__':
 
    db_file_name = r"C:\temp\sqlite\pythonsqlite.db"
    create_file(db_file_name)
    conn = create_connection(db_file_name)
    create_table(conn)
    insert_records(conn)
 
    conn.close()

