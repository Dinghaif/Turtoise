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