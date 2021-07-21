Git is a version control system.
git is free software.

一、创建版本库
mkdir Git (创建版本库)
git init （把目录变成Git可以管理的仓库）
二、把文件添加进版本库
git add readme.txt(把文件添加到仓库)
git commit -m "write a readme file"(告诉Git，把文件提交到仓库)

git status （查看仓库当前的状态）
git diff 文件 （查看具体修改了什么）

git reset --hard commit_id(穿梭版本)
git log(查看提交历史)
git reflog（查看命令历史）
git checkout -- file （丢弃对工作区的修改）
git reset head file 然后再git checkout -- file (丢弃对暂存区的修改)
git rm file （删除一个文件）

拥有本地库后关联远程库操作
1、创建SSH Key
ssh-keygen -t rsa -C "youremail@example.com"
2、将生成的公钥放在你的GitHub账户中

三、将本地内容推送到GitHub远程仓库
在本地仓库下运行命令
1、git remote add origin git@github.com:account/远程仓库.git
--origin 远程库的名字就是origin
2、git push -u origin master
--u Git不但会把本地的master分支内容推送的远程新的master分支，还会把本地的master分支和远程的master分支关联起来
3、之后推送到远程仓库就使用
git push origin master

先有远程库，后克隆本地库
1、登录GitHub，创建一个新的仓库名称repository
2、用命令克隆一个本地库
git clone git@github.com:account/repository.git

git remote rm origin
--删除远程库


Gitee上传自己的SSH公钥，操作和GitHub一样

详细教程https://www.liaoxuefeng.com/
