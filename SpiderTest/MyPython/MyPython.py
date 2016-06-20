#The Zen Of Python
#import this

#引用包中某个具体的变量或者函数
#from random import randint

#普通引用
#import requests
#response=requests.get("http://www.jb51.net/article/63711.htm","W")
#import BeautifulSoup
#soup=BeautifulSoup.BeautifulSoup(response.text)
#print(soup.findAll('class'))
#print(soup.Name)

#def digui(x):
#    if x<1000:
#        print(x)
#        x=x+1
#        digui(x)
#    else:
#        print("down")

#arrylist=[1,2,3,4,5,6,7,8,9,10]
#print arrylist[1]
#arrylist.pop(2)
#print arrylist
#print len(arrylist)

#def x(x,y):
#    z=x*y
#    return z
#print x(3,8)
#import this

str="abc"
help(str.center)
print str[1:]
#python 的一大特色就是对字符串的操作
#和javascript类似python不区分"和'符号，这也省去了有时候需要转义的操作

def x(*x):
    return x

r=x("1","2")
print r

#python的lambda表达式
x=lambda x,y:x*y
print x(3,2)

#n("a")

#面向对象
class test():
    def __init__(self):
        print "- -."

#实例化
instence=test()
print (instence.__init__())

#读取文件
f=file("D:\Python27\LICENSE.txt")
print f.readline()
f.close()

#写入文件  文件名必须是大写  不知道为什么啊
c=open("D:\Python27\TEST.txt",'w')
c.write(data)
c.close()

#异常捕获
try:
    file("D:\Python27\shit.txt",'w').read()
except:
    print "application go die"

#查看此类包含的属性函数等
print dir(random)

#接受任意数量的参数
#在变量前加上星号前缀（*），
#调用时的参数会存储在一个 tuple（元组）对象中，赋值给形参。在函数内部，需要对参数进行处理时，
#只要对这个 tuple 类型的形参（这里是 args）进行操作就可以了。因此，函数在定义时并不需要指明参数个数，就可以处理任意参数个数的情况。
#不过有一点需要注意，tuple 是有序的，所以 args 中元素的顺序受到赋值时的影响。
def func(*args):
    for i in args:
        print i

func(1,2,3,4,5,6,67,8)


#func(**kargs) 是把参数以键值对字典的形式传入。
def printAll(**kargs):
	for k in kargs:
		print k, ':', kargs[k]
printAll(a=1, b=2, c=3)


#python 里有一个 thread 模块，其中提供了一个函数：
#start_new_thread(function, args[, kwargs])
#function 是开发者定义的线程函数，
#args 是传递给线程函数的参数，必须是tuple类型，
#kwargs 是可选参数

