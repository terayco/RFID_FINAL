from flask import jsonify


def success_api(msg: str="成功", data={}):
    """ 成功响应 默认值”成功“ """
    return jsonify(success=True, code=0, msg=msg, data=data)


def fail_api(msg: str="失败"):
    """ 失败响应 默认值“失败” """
    return jsonify(success=False, code=1, msg=msg)