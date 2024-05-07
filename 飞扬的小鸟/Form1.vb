Imports System.Media

Public Class Form1
    Public Shared move_speed As Single = 3.0F '用户当前移动速度'
    Public Shared zhongli As Single = 0.25 '重力大小'
    Public start As Boolean = False '游戏是否开始

    Private background As Image = Image.FromFile("img/bg.png")
    Private ground As Image = Image.FromFile("img/ground.png")
    Private ground_x As Single = 0F
    Private ground_y As Single = 498.0F
    Private play As Player = New Player(40, 80) '玩家小鸟'
    Private overImage As Image = Image.FromFile("img/gameover.png") '游戏结束'
    Private startImage As Image = Image.FromFile("img/start.png") '游戏开始图片
    Private zhuzi1 As Column = New Column(400) '第一个柱子 上限: -200 上限-470  
    Private zhuzi2 As Column '第二个柱子
    Private time As Integer = 0

    Private backgroundMusic As New SoundPlayer("music/music.wav")
    Private clickMusic As New SoundPlayer("music/jump.wav")


    Private scoreTimer As New Stopwatch() ' 计时器用于记录成绩
    Private currentScore As Integer = 0 ' 当前成绩
    Private highScore As Integer = 0 ' 最高分
    Private addspeed = 0.1F

    Private wudi = False


    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True) '防止画面闪烁'
        Me.UpdateStyles()
        Me.Size = New Drawing.Size(432, 644 + 38) '设置窗体尺寸, 38是窗体标题的一个高度'
        backgroundMusic.Load()
        ' 开始播放背景音乐，并设置循环播放

        backgroundMusic.PlayLooping()

        zhuzi2 = New Column(400 + (432 + zhuzi1.width) / 2)
    End Sub

    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        e.Graphics.DrawImage(background, 0, 0, background.Width, background.Height)
        zhuzi1.draw(e) : zhuzi2.draw(e) '绘制柱子
        e.Graphics.DrawImage(ground, ground_x, ground_y, ground.Width * 0.95F, ground.Height * 0.95F) '绘制地面'
        play.draw(e) '绘制小鸟

        If Player.life = False Then
            e.Graphics.DrawImage(overImage, 0, 0, background.Width, background.Height) '绘制游戏结束图
        End If
        If start = False Then
            e.Graphics.DrawImage(startImage, 0, 0, background.Width, background.Height) '绘制开始界面
        End If

        ' 如果游戏开始且小鸟还活着，则绘制当前成绩
        If start AndAlso Player.life Then
            scoreTimer.Start()
            currentScore = CInt(scoreTimer.ElapsedMilliseconds) / 100 ' 将毫秒转换为整数成绩
            Dim scoreFont As New Font("Arial", 24, FontStyle.Bold)
            Dim scoreBrush As New SolidBrush(Color.White)
            e.Graphics.DrawString(currentScore.ToString(), scoreFont, scoreBrush, 10, 10)
        ElseIf Player.life = False AndAlso start Then
            ' 游戏结束，绘制游戏结束图和最终成绩


            e.Graphics.DrawImage(overImage, 0, 0, background.Width, background.Height)
            Dim finalScoreFont As New Font("Arial", 26, FontStyle.Bold)
            Dim finalScoreBrush As New SolidBrush(Color.Red)
            Dim scoreText As String = "最终成绩: " & currentScore.ToString()

            Dim scoreSize As SizeF = e.Graphics.MeasureString(scoreText, finalScoreFont)
            Dim scoreLocation As New PointF((Me.Width - scoreSize.Width) / 2, (Me.Height - scoreSize.Height) / 2)
            e.Graphics.DrawString(scoreText, finalScoreFont, finalScoreBrush, scoreLocation)

            scoreTimer.Reset()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If Player.life And start Then '如果小鸟还活着并且游戏开始'

            '让地面移动'
            ground_x -= move_speed
            If ground_x < -67.8F Then
                ground_x = 0F
            End If

            '让小鸟移动
            play.run()

            '让柱子移动
            zhuzi1.run(wudi)
            zhuzi2.run(wudi)
            '判断小鸟是否落地


            If Player.y + Player.height > ground_y And Player.life Then
                If wudi = False Then
                    Player.life = False '小鸟死亡'
                    scoreTimer.Stop()
                    'MsgBox("重开吧骚年")
                ElseIf wudi Then
                    Player.life = True
                End If


            End If

            '让小鸟移动速度渐渐增加
            time += 1
            If time > 10 Then
                time = 0
                move_speed += addspeed
            End If

        End If
        '第一种：

        '        Me.frmChild.Requery

        '        这是最有效， 最简单的方法。

        '第二种：

        '        Me.frmChild.Form.Refresh

        '        这种方法并不能使窗体中的数据立即改变。

        '第三种：

        '        Me.frmChild.SourceObject = “”
        '       DoCmd.RunSQL strSQL
        'Me.frmChild.SourceObject = “frmsalelist_child”
        '这种方法的效果等同于第一种。
        Me.Refresh() '刷新paint方法'
    End Sub


    Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp '鼠标松开'


        If start = False Then
            start = True '游戏开始

        ElseIf Player.life = False Then '如果小鸟死亡
            '初始化
            Player.y = 80
            play.drop_speed = -5.0F
            move_speed = 3.0F
            zhuzi1.x = 400 : zhuzi2.x = 400 + (432 + zhuzi2.width) / 2
            Player.life = True
            currentScore = 0
            ' 开始游戏时启动计时器
            scoreTimer.Start()



        Else
            play.drop_speed = -6.0F
        End If

        play.drop_speed = -6.0F

        If start = True Then

        ElseIf Player.life = False Then
            ' 小鸟死亡时停止计时器

            ' 更新最高分

            ' 重置成绩和计时器

            ' 游戏结束，显示最终成绩和最高分
            'MsgBox("游戏结束" & vbCrLf & "最终成绩是: " & currentScore & " 毫秒" & vbCrLf & "最高分是: " & highScore & " 毫秒")
            ' 重新开始游戏

        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        move_speed = 4.0F
        addspeed = 0.3F
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        move_speed = 8.0F
        addspeed = 0.5F
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        move_speed = 1.0F
        addspeed = 0.1F
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        wudi = Not wudi

    End Sub
End Class
