<body>
   ##Документација за играта "Color Switch Game"

    <h2>1. Објаснување на проблемот</h2>

    <h3>Опис на апликацијата</h3>
    <p>"Color Switch Game" е забавна и предизвикувачка игра каде што играчот контролира топче кое постојано скока нагоре. Целта на играта е да се избегнат пречките и да се соберат што повеќе поени. Играчот може да го контролира скокот на топчето со притискање на тастерот "Space". Пречките имаат различни форми и движења, и играчот мора да ги избегне за да преживее.</p>

    <h3>Функционалности</h3>
    <ul>
        <li>Старт на играта: Играчот може да ја започне играта со притискање на копчето "Start".</li>
        <li>Пауза и продолжување: Играчот може да ја паузира и продолжи играта со притискање на копчето "Pause/Resume".</li>
        <li>Контрола на топчето: Со притискање на тастерот "Space", топчето скока.</li>
        <li>Пречки: Постојат пречки кои играчот мора да ги избегне.</li>
        <li>Собирање поени: Играчот собира поени кога успешно ќе избегне пречки.</li>
    </ul>

    <h3>Како се игра</h3>
    <p>Играчот го контролира топчето со притискање на тастерот "Space", кое го прави топчето да скокне нагоре. Целта е да се избегнат пречките кои се движат или ротираат на начин што ќе ги совпадне бојата на топчето и на дел од препреката, и да се соберат што повеќе поени. Играчот може да ја паузира играта во било кое време со притискање на копчето "Pause", и да ја продолжи со притискање на "Resume". Секои 10 секунди топчето добива random боја.</p>

    <h2>2. Чување на податоци</h2>
    <p>Податоците се чуваат во класи кои ги дефинираат различните елементи на играта:</p>
    <ul>
        <li><b>Scene:</b> Главната класа која ги содржи сите елементи на играта (топчето, пречките) и управува со логиката на играта.</li>
        <li><b>Ball:</b> Класа која го претставува моделот за топчето, вклучувајќи ги неговите својства како позиција, брзина, и методите за негово движење.</li>
        <li><b>Obstacle:</b> Класа која го дефинира моделот и карактеристиките на пречките. Потоа, како се креираат пречки и некои битни функционалности како што е справување со колизија со топчето.</li>
        <li><b>GameForm:</b> Самата класа на формата која ги содржи функциите за справување со различните евенти кои корисникот може да ги направи како и иницијализација на играта.</li>
    </ul>

    <h2>3. Опис на класата Obstacle.cs</h2>

    <h3>Опис на функциите</h3>
    <h4>1. Конструктор: Obstacle(Point center, int radius, Color[] colors)</h4>
    <p>Овој конструктор го иницијализира објектот на класата Obstacle со зададениот центар, радиус и бои. Исто така, го поставува аголот на ротација на нула и го креира објектот за поени кој играчот може да го собере.</p>
    <pre><code>
public Obstacle(Point center, int radius, Color[] colors)
{
    Center = center;
    Radius = radius;
    Colors = colors;
    RotationAngle = 0;
    ScoreObject = new Rectangle(Center.X - 10, Center.Y - 10, 20, 20);
    ScoreObjectCollected = false;
}
    </code></pre>

    <h4>2. Update()</h4>
    <p>Оваа функција ја ажурира пречката со зголемување на аголот на ротација. Ова значи дека пречката ќе ротира со секое повикување на оваа функција.</p>
    <pre><code>
public void Update()
{
    RotationAngle = (RotationAngle + 2) % 360; 
}
    </code></pre>
    <p><b>Објаснување:</b><br>
    <code>RotationAngle = (RotationAngle + 2) % 360;</code> Ова го зголемува аголот на ротација за 2 степени и правиме модул со 360 за да се осигураме дека аголот останува во опсегот од 0 до 360 степени.</p>

    <h4>3. Draw(Graphics g)</h4>
    <p>Оваа функција ја црта пречката на даден графички објект. Таа ги користи боите од низата Colors за да ги нацрта различните сегменти на пречката и ако објектот за поени не е собран, ја црта и ѕвездата која го претставува објектот за поени.</p>
    <pre><code>
public void Draw(Graphics g)
{
    float sweepAngle = 360f / Colors.Length;
    for (int i = 0; i < Colors.Length; i++)
    {
        using (Pen pen = new Pen(Colors[i], PenWidth)) // Use pen instead of brush for drawing the perimeter
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2, RotationAngle + i * sweepAngle, sweepAngle);
                g.DrawPath(pen, path);
            }
        }
    }

    if (!ScoreObjectCollected)
    {
        using (GraphicsPath starPath = CreateStarPath(ScoreObject.X + ScoreObject.Width / 2,   
                ScoreObject.Y + ScoreObject.Height / 2, ScoreObject.Width / 2, ScoreObject.Width / 4, 5))
        {
            using (Brush brush = new SolidBrush(Color.NavajoWhite))
            {
                g.FillPath(brush, starPath);
            }
        }
    }
}
    </code></pre>
    <p><b>Објаснување:</b><br>
    <b>Цртање на пречката:</b><br>
    <code>float sweepAngle = 360f / Colors.Length;</code> Се пресметува аголот за секој сегмент врз основа на бројот на бои.<br>
    Во <code>for</code> циклусот се користи <code>Pen</code> за да се црта секој сегмент на пречката со соодветна боја и агол на ротација.<br>
    <code>path.AddArc()</code> додава арка на патеката која ќе биде нацртана.<br>
    <code>g.DrawPath(pen, path);</code> ја црта патеката на графичкиот објект.<br>
    <b>Цртање на објектот за поени:</b><br>
    <code>CreateStarPath()</code> создава патека во форма на ѕвезда.<br>
    <code>g.FillPath(brush, starPath);</code> ја пополнува ѕвездата со зададената боја ако објектот за поени не е собран.</p>

    <h4>4. CheckCollision(Ball ball)</h4>
    <p>Оваа функција проверува дали топчето се судрило со пречката. Функцијата го проверува растојанието од центарот на топчето до центарот на пречката и го споредува со радиусот на пречката за да одреди дали топчето е на периферијата на пречката. Потоа, го проверува аголот до топчето и соодветната боја на сегментот за да види дали бојата на топчето се совпаѓа со бојата на сегментот.</p>
    <pre><code>
public bool CheckCollision(Ball ball)
{
    float distance = (float)Math.Sqrt(Math.Pow(ball.X - Center.X, 2) + Math.Pow(ball.Y - Center.Y, 2));
    bool onPerimeter = Math.Abs(distance - Radius) <= PenWidth / 2;

    if (!onPerimeter)
    {
        return false;
    }

    float angleToBall = (float)(Math.Atan2(ball.Y - Center.Y, ball.X - Center.X) * 180 / Math.PI);
    angleToBall = (angleToBall + 360) % 360;

    float segmentAngle = 360f / Colors.Length;
    int segment = (int)((angleToBall - RotationAngle + 360) % 360 / segmentAngle);

    bool colorMatch = Colors[segment] == ball.Color;

    return !colorMatch;
}
    </code></pre>
    <p><b>Објаснување:</b><br>
    <b>Пресметка на растојание:</b><br>
    <code>float distance = (float)Math.Sqrt(Math.Pow(ball.X - Center.X, 2) + Math.Pow(ball.Y - Center.Y, 2));</code> Пресметува евклидово растојание од центарот на топчето до центарот на пречката.<br>
    <b>Проверка на периферијата:</b><br>
    <code>bool onPerimeter = Math.Abs(distance - Radius) <= PenWidth / 2;</code> Проверува дали топчето е на периферијата на пречката со споредување на растојанието со радиусот и ширината на перото.<br>
    <b>Проверка на боја:</b><br>
    <code>float angleToBall = (float)(Math.Atan2(ball.Y - Center.Y, ball.X - Center.X) * 180 / Math.PI);</code> Пресметува агол од центарот на пречката до топчето.<br>
    <code>angleToBall = (angleToBall + 360) % 360;</code> Нормализирање на аголот во опсег од 0 до 360 степени.<br>
    <code>int segment = (int)((angleToBall - RotationAngle + 360) % 360 / segmentAngle);</code> Одредување на сегментот врз основа на аголот.<br>
    <code>bool colorMatch = Colors[segment] == ball.Color;</code> Проверка дали бојата на топчето се совпаѓа со бојата на сегментот.<br>
    <b>Враќање на резултат:</b><br>
    <code>return !colorMatch;</code> Враќа true ако бојата не се совпаѓа, што значи дека се случува судир.</p>

    <h4>5. CheckOutOfBounds(Ball ball)</h4>
    <p>Оваа функција проверува дали топчето излегло од границите на играта, што значи дека е под дното на прозорецот.</p>
    <pre><code>
public bool CheckOutOfBounds(Ball ball)
{
    if (ball.Y + 10 > 1000)
    {
        return true;
    }
    return false;
}
    </code></pre>
    <p><b>Објаснување:</b><br>
    <b>Проверка на граници:</b><br>
    <code>if (ball.Y + 10 > 1000):</code> Проверува дали топчето е под дното на прозорецот (со мала маргина).<br>
    <code>return true;</code> Враќа true ако топчето е надвор од границите.</p>

    <h4>6. CheckScoreObjectCollision(Ball ball)</h4>
    <p>Оваа функција проверува дали топчето го собрало објектот за поени. Ако топчето е во рамките на објектот за поени, функцијата го означува објектот како собран и враќа true.</p>
    <pre><code>
public bool CheckScoreObjectCollision(Ball ball)
{
    if (!ScoreObjectCollected && ScoreObject.Contains(ball.X, ball.Y))
    {
        ScoreObjectCollected = true;
        return true;
    }
    return false;
}
    </code></pre>
    <p><b>Објаснување:</b><br>
    <b>Проверка на колизија со објект за поени:</b><br>
    <code>if (!ScoreObjectCollected && ScoreObject.Contains(ball.X, ball.Y)):</code> Проверува дали објектот за поени не е собран и дали топчето е во рамките на објектот за поени.<br>
    <code>ScoreObjectCollected = true;</code> Означува дека објектот за поени е собран.<br>
    <code>return true;</code> Враќа true ако топчето го собрало објектот за поени.</p>

</body>
</html>
