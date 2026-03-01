from functools import wraps


def non_empty(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        result = func(*args, **kwargs)
        if isinstance(result, list):
            return [x for x in result if x is not None and x != '']
        if isinstance(result, tuple):
            return tuple(x for x in result if x is not None and x != '')
        return result

    return wrapper


@non_empty
def get_pages():
    return ['chapter1', '', 'contents', None, 'line1']


print(get_pages())
