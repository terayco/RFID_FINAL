from datetime import datetime
from flask import Flask, request
from flask_cors import CORS
import config
from exts import db
from flask_migrate import Migrate
from utils.http import success_api,fail_api
from model import *

#创建app对象
app = Flask(__name__)
# 加载配置文件
app.config.from_object(config)
# db绑定app
db.init_app(app)
#表的映射
migrate = Migrate(app, db)
# 设置跨域
CORS(app, resources=r'/*', supports_credentials=True)

def insert():
    information = Info("2062410117","易博坤","男",110.00,"1")
    db.session.add(information)
    db.session.commit()
    return 'success'

# with app.app_context():
#     with db.engine.connect() as conn:
#          # info = Info.query.filter(Info.member_ID == "2062410124").first()
#          # print(info)
#          # print(info.sex)
#          record = Records(member_ID="2062410124", operation_type="1",
#                           operation_money=15.00, operation_time=datetime.now())
#          db.session.add(record)
#          db.session.commit()

@app.route('/money',methods=['POST'])
def moneyFunction():
    '''
    存钱
    :return: 成功信息
    '''
    member_ID = request.get_json().get('member_ID')
    operation_type = request.get_json().get('operation_type')
    operation_money = float(request.get_json().get('operation_money'))

    record = Records(member_ID=member_ID, operation_type=operation_type,
                          operation_money=operation_money, operation_time=datetime.now())
    if operation_type == "0":
        Info.query.filter(Info.member_ID == member_ID).update({'balance': operation_money + Info.balance})
    else :
        Info.query.filter(Info.member_ID == member_ID).update({'balance': operation_money - Info.balance})

    try:
        db.session.add(record)
        db.session.commit()
        return success_api()
    except Exception as e:
        print(e)
        return fail_api("操作金额必须是大于0的数！")


@app.route('/insert',methods=['POST'])
def addMember():
    '''
    添加员工
    :return: 成功或失败信息
    '''
    member_ID = request.get_json().get('member_ID')
    member_name = request.get_json().get('member_name')
    sex = request.get_json().get('sex')
    age = request.get_json().get('age')
    balance = float(request.get_json().get('balance'))
    card_status = request.get_json().get('card_status')
    info = Info(member_ID, member_name, sex, age, balance, card_status)
    try:
        db.session.add(info)
        db.session.commit()
        return success_api()
    except Exception as e:
        print(e)
        return fail_api('添加失败，该员工已存在!')

@app.route('/edit',methods=['POST'])
def editInfo():
    ID = int(request.get_json().get('ID'))
    member_ID = request.get_json().get('member_ID')
    member_name = request.get_json().get('member_name')
    sex = request.get_json().get('sex')
    age = request.get_json().get('age')
    balance = float(request.get_json().get('balance'))
    card_status = request.get_json().get('card_status')
    Info.query.filter(Info.ID == ID).update({'member_ID':member_ID,
                                                     'member_name':member_name, 'sex':sex,
                                                           'age':age, 'balance':balance,
                                                           'card_status':card_status})
    try:
        db.session.commit()
        return success_api()
    except Exception as e:
        print(e)
        return fail_api("修改失败！")

@app.route('/delete',methods=['POST'])
def deleteInfo():
    member_ID = request.get_json().get('member_ID')
    Info.query.filter(Info.member_ID == member_ID).delete()
    try:
        db.session.commit()
        return success_api()
    except Exception as e:
        print(e)
        return fail_api("删除失败！")

@app.route('/memberInfo',methods=['GET'])
def memberinfo():
    '''
    获取信息
    :return: 数据库所有数据
    '''
    members = Info.query.filter(Info.member_ID != "").all()
    member_list = []
    for member in members:
        member_dic = {}
        member_dic['ID'] = member.ID
        member_dic['member_ID'] = member.member_ID
        member_dic['member_name'] = member.member_name
        member_dic['sex'] = member.sex
        member_dic['age'] = member.age
        member_dic['balance'] = member.balance
        member_dic['card_status'] = member.card_status
        member_list.append(member_dic)

    return success_api(data=member_list)

@app.route('/recordInfo',methods=['GET'])
def recordInfo():
    records = Records.query.filter(Records.ID != "").all()
    record_list = []
    for record in records:
        record_dic = {}
        record_dic['ID'] = record.ID
        record_dic['member_ID'] = record.member_ID
        record_dic['operation_type'] = record.operation_type
        record_dic['operation_money'] = record.operation_money
        record_dic['operation_time'] = str(record.operation_time)
        record_dic['member_name'] = Info.query.filter(Info.member_ID == record.member_ID).first().member_name
        record_list.append(record_dic)
    return success_api(data=record_list)

@app.route('/initInfo',methods=['POST'])
def initInfo():
    '''
    初始化卡信息
    :return: 成功或失败信息
    '''
    member_ID = request.get_json().get('member_ID')
    member_name = request.get_json().get('member_name')
    sex = request.get_json().get('sex')
    age = request.get_json().get('age')
    balance = float(request.get_json().get('balance'))
    card_status = request.get_json().get('card_status')

    if(Info.query.filter(Info.member_ID == member_ID).first()!=None):
        try:
            Info.query.filter(Info.member_ID == member_ID).update({
                                                     'member_name': member_name, 'sex': sex,
                                                     'age': age, 'balance': balance,
                                                     'card_status': card_status})
            db.session.commit()
            return success_api()
        except Exception as e:
            print(e)
            return fail_api('失败')
    else:
        try:
            info = Info(member_ID, member_name, sex, age, balance, card_status)
            db.session.add(info)
            db.session.commit()
            return success_api()
        except Exception as e:
            print(e)
            return fail_api('失败')

if __name__ == '__main__':
    app.run(host='0.0.0.0',port=5000)
