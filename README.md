<h1>Документација за играта "Color Switch"</h1>

<h2>Опис на апликацијата </h2>
"Color Switch Game" е забавна и предизвикувачка игра каде што играчот контролира топче кое постојано скока нагоре. Целта на играта е да се избегнат пречките и да се соберат што повеќе поени. Играчот може да го контролира скокот на топчето со притискање на тастерот "Space". Пречките имаат различни форми и движења, и играчот мора да ги избегне за да преживее.

<h2>Функционалности </h2>
Старт на играта: Играчот може да ја започне играта со притискање на копчето "Start".
Пауза и продолжување: Играчот може да ја паузира и продолжи играта со притискање на копчето "Pause/Resume".
Контрола на топчето: Со притискање на тастерот "Space", топчето скока.
Пречки: Постојат пречки кои играчот мора да ги избегне.
Собирање поени: Играчот собира поени кога успешно ќе избегне пречки.
Како се игра
Играчот го контролира топчето со притискање на тастерот "Space", кое го прави топчето да скокне нагоре. Целта е да се избегнат пречките кои се движат или ротираат на начин што ќе ги совпадне бојата на топчето и на дел од препреката, и да се соберат што повеќе поени. Играчот може да ја паузира играта во било кое време со притискање на копчето "Pause", и да ја продолжи со притискање на "Resume".
Секои 10 секунди топчето добива random боја. 

<h2>Чување на податоци </h2>
Податоците се чуваат во класи кои ги дефинираат различните елементи на играта:
Scene: Главната класа која ги содржи сите елементи на играта (топчето, пречките) и управува со логиката на играта.
Ball: Класа која го претставува моделот за топчето, вклучувајќи ги неговите својства како позиција, брзина, и методите за негово движење.
Obstacle: Класа која го дефинира моделот и карактеристиките на пречките. Потоа, како се креираат пречки и некои битни функционалности како што е справување со колизија со топчето.
GameForm: Самата класа на формата која ги содржи функциите за справување со различните евенти кои корисникот може да ги направи како и иницијализација на играта.

<h2>Опис на класата Obstacle.cs </h2>

Опис на функциите
<h2> </h2>1. Конструктор: Obstacle(Point center, int radius, Color[] colors)
Овој конструктор го иницијализира објектот на класата Obstacle со зададениот центар, радиус и бои. Исто така, го поставува аголот на ротација на нула и го креира објектот за поени кој играчот може да го собере.

public Obstacle(Point center, int radius, Color[] colors)
{
    Center = center;
    Radius = radius;
    Colors = colors;
    RotationAngle = 0;
    ScoreObject = new Rectangle(Center.X - 10, Center.Y - 10, 20, 20);
    ScoreObjectCollected = false;
}

<h2>2. Update() </h2>
Оваа функција ја ажурира пречката со зголемување на аголот на ротација. Ова значи дека пречката ќе ротира со секое повикување на оваа функција.

public void Update()
{
    RotationAngle = (RotationAngle + 2) % 360; 
}
Објаснување:
RotationAngle = (RotationAngle + 2) % 360;: Ова го зголемува аголот на ротација за 2 степени и правиме модул со 360 за да се осигураме дека аголот останува во опсегот од 0 до 360 степени.

<h2>3. Draw(Graphics g) </h2>
Оваа функција ја црта пречката на даден графички објект. Таа ги користи боите од низата Colors за да ги нацрта различните сегменти на пречката и ако објектот за поени не е собран, ја црта и ѕвездата која го претставува објектот за поени.

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
Објаснување:
Цртање на пречката:

float sweepAngle = 360f / Colors.Length; Се пресметува аголот за секој сегмент врз основа на бројот на бои.
Во for циклусот се користи Pen за да се црта секој сегмент на пречката со соодветна боја и агол на ротација.
path.AddArc() додава арка на патеката која ќе биде нацртана.
g.DrawPath(pen, path); ја црта патеката на графичкиот објект.
Цртање на објектот за поени:
CreateStarPath() создава патека во форма на ѕвезда.
g.FillPath(brush, starPath); ја пополнува ѕвездата со зададената боја ако објектот за поени не е собран.

<h2>4. CheckCollision(Ball ball) </h2>
Оваа функција проверува дали топчето се судрило со пречката. Функцијата го проверува растојанието од центарот на топчето до центарот на пречката и го споредува со радиусот на пречката за да одреди дали топчето е на периферијата на пречката. Потоа, го проверува аголот до топчето и соодветната боја на сегментот за да види дали бојата на топчето се совпаѓа со бојата на сегментот.

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
Објаснување:
Пресметка на растојание:

float distance = (float)Math.Sqrt(Math.Pow(ball.X - Center.X, 2) + Math.Pow(ball.Y - Center.Y, 2)); Пресметува евклидово растојание од центарот на топчето до центарот на пречката.
Проверка на периферијата:
bool onPerimeter = Math.Abs(distance - Radius) <= PenWidth / 2; Проверува дали топчето е на периферијата на пречката со споредување на растојанието со радиусот и ширината на перото.
Проверка на боја:
float angleToBall = (float)(Math.Atan2(ball.Y - Center.Y, ball.X - Center.X) * 180 / Math.PI); Пресметува агол од центарот на пречката до топчето.
angleToBall = (angleToBall + 360) % 360; Нормализирање на аголот во опсег од 0 до 360 степени.
int segment = (int)((angleToBall - RotationAngle + 360) % 360 / segmentAngle); Одредување на сегментот врз основа на аголот.
bool colorMatch = Colors[segment] == ball.Color; Проверка дали бојата на топчето се совпаѓа со бојата на сегментот.
Враќање на резултат:
return !colorMatch; Враќа true ако бојата не се совпаѓа, што значи дека се случува судир.

<h2>5. CheckOutOfBounds(Ball ball)</h2>
Оваа функција проверува дали топчето излегло од границите на играта, што значи дека е под дното на прозорецот.

public bool CheckOutOfBounds(Ball ball)
{
    if (ball.Y + 10 > 1000)
    {
        return true;
    }
    return false;
}
Објаснување:
Проверка на граници:
if (ball.Y + 10 > 1000): Проверува дали топчето е под дното на прозорецот (со мала маргина).
return true; Враќа true ако топчето е надвор од границите.

<h2>6. CheckScoreObjectCollision(Ball ball) </h2>
Оваа функција проверува дали топчето го собрало објектот за поени. Ако топчето е во рамките на објектот за поени, функцијата го означува објектот како собран и враќа true.

public bool CheckScoreObjectCollision(Ball ball)
{
    if (!ScoreObjectCollected && ScoreObject.Contains(ball.X, ball.Y))
    {
        ScoreObjectCollected = true;
        return true;
    }
    return false;
}
Објаснување:
Проверка на колизија со објект за поени:
if (!ScoreObjectCollected && ScoreObject.Contains(ball.X, ball.Y)): Проверува дали објектот за поени не е собран и дали топчето е во рамките на објектот за поени.
ScoreObjectCollected = true; Означува дека објектот за поени е собран.
return true; Враќа true ако топчето го собрало објектот за поени.

<h2>Screenshots of the game</h2>

![Screenshot 2024-06-21 133335](https://github.com/dariyozz/Color-Switch-Game/assets/134236483/414d0bd5-e41f-4635-8212-55b5bcd268e7)
![Screenshot 2024-06-21 133427](https://github.com/dariyozz/Color-Switch-Game/assets/134236483/87876050-0380-4bb5-b4d5-99b89393d4c1)
![Screenshot 2024-06-21 133503](https://github.com/dariyozz/Color-Switch-Game/assets/134236483/5304241b-5aa9-4e3e-94a9-357645d65459)

