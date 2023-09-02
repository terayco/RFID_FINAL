from exts import db

class Info(db.Model):
    __tablename__ = 'info'
    ID = db.Column(db.Integer, primary_key=True, autoincrement=True)
    member_ID = db.Column(db.String(20), nullable = False,unique = True)
    member_name = db.Column(db.String(10), nullable=False)
    sex = db.Column(db.String(2), nullable=False)
    age = db.Column(db.String(2), nullable=False)
    balance = db.Column(db.Numeric(10,2), nullable=False)
    card_status = db.Column(db.String(2))
    # records = db.relationship('Records', backref='infos',
    #                           lazy='dynamic')

    def __init__(self, member_ID, member_name, sex, age, balance, card_status):
        self.member_ID = member_ID
        self.member_name =  member_name
        self.sex = sex
        self.age = age
        self.balance = balance
        self.card_status = card_status

    def __repr__(self):
        return '<Post %r>' % self.member_name

class Records(db.Model):
    __tablename__ = 'records'
    ID = db.Column(db.Integer, primary_key=True, autoincrement=True)
    member_ID = db.Column(db.String(20), nullable=False)
    operation_type = db.Column(db.String(2), nullable=False)
    operation_money = db.Column(db.Numeric(10,2), nullable=False)
    operation_time = db.Column(db.DateTime, nullable=False)

    def __init__(self, member_ID, operation_type, operation_money, operation_time):
        self.member_ID =  member_ID
        self.operation_type = operation_type
        self.operation_money = operation_money
        self.operation_time = operation_time

    def __repr__(self):
        return '<Post %r>' % self.ID