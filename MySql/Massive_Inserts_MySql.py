#!/usr/bin/python

import sys
import mysql.connector
import uuid
import string
import random
import time
 

date_time_format = "%Y/%m/%d"
create_table = False

def create_connection():
    """ create a database connection to a MySQL, mariaDB database """

    try:
        conn = mysql.connector.connect(
                host="192.168.1.9",
                port=6606,
                user="rootx",
                password="maria",
                database="TeraBase")
    except mysql.connector.Error as e:
        print(e)
        raise SystemExit
        sys.exit(1)

    return conn
 

def create_table(conn):
    """ create a table from the create_table_sql statement
    :param conn: Connection object
    :return:
    """

    try:
        mycursor = conn.cursor()
        mycursor.execute("SHOW DATABASES")
        for x in mycursor:
            print(x)

        sql = """CREATE TABLE IF NOT EXISTS products (
            id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            guid VARCHAR(36) NOT NULL, 
            name VARCHAR(255) NOT NULL,
            price INT NOT NULL,
            validation VARCHAR(10) NOT NULL
            );"""

        ccursor = conn.cursor()
        ccursor.execute(sql)
    
        conn.commit()
        ccursor.close()

    except mysql.connector.Error as e:
        print(e)
        raise SystemExit
        sys.exit(1)
 

def insert_record(conn, guid, name, price, validation):
    """ insert record in table from the insert_sql statement
    :param conn: Connection object
    :return:
    """

    sql = "INSERT INTO products VALUES (NULL, '" + guid + "', '" + name + "', '" + str(price) + "', '" + validation + "');"
    #sql= "INSERT INTO users (name, user_name) VALUES (%s, %s)"
    #values = ("Hafeez", "hafeez")
    #cursor.execute(query, values)

    try:
        ccursor = conn.cursor()
        ccursor.execute(sql)
        conn.commit()
        #print(ccursor.rowcount, "records inserted")
    except mysql.connector.Error as e:
        print(e)
        raise SystemExit
        sys.exit(1)
 

def insert_records(conn):
    """ insert a range of records in table
    :param conn: Connection object
    :return:
    """

    for x in range(2):
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

    prop = random.random()
    stime = time.mktime(time.strptime("2000/01/01", date_time_format))
    etime = time.mktime(time.strptime("2050/12/31", date_time_format))
    ptime = stime + prop * (etime - stime)

    return time.strftime(date_time_format, time.localtime(ptime))
 

if __name__ == '__main__':
    
    conn = create_connection()
    if (create_table):
        create_table(conn)
    insert_records(conn)
    conn.close()
