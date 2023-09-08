using AiSD5; 
using System;
using System.Collections.Generic;
using System.Windows.Forms; 

namespace AiSD6 
{
    public partial class Form1 : Form 
    {
        public Form1() 
        {
            InitializeComponent(); // 
        }

        private List<MyQueue> StationQueues; // Объявление списка очередей станций
        private List<Train> Trains; // Объявление списка поездов

        private void timer1_Tick(object sender, EventArgs e)//Тик
        {
            numberOfPeople = random.Next(0, maxNumberOfPeople); // Генерация случайного числа людей

            for (int i = 0; i < numberOfPeople; i++)
            {
                Person person = new Person(); 
                int numberofstation = random.Next(0, numberOfStations); // Генерация случайного числа станции
                StationQueues[numberofstation].push(person); // Человек встаёт в случайную очередь
                person.setQueue(StationQueues[numberofstation]); // Установка очереди для человека
            }

            timeOfWork++; // Увеличение времени работы на 1 тик

            for (int i = 0; i < Trains.Count; i++)
            {
                Train train = Trains[i];
                train.decTime();

                if (train.getTimer() == (numberOfStations - 1) * 20) // Если время поезда равно (число станций - 1) * 20
                {
                    int max = Math.Min(train.getMaxCapacity(), StationQueues[0].getSize()); // Максимальное количество пассажиров, которые могут войти в поезд
                    int number = random.Next(0, max); // Генерация случайного числа пассажиров
                    for (int j = 0; j < number; j++) 
                    {
                        train.pushPassenger((Person)StationQueues[0].pop()); // Посадка пассажиров
                    }
                    continue;
                }

                if (train.getTimer() % 20 == 0) // Если время поезда кратно 20 (поезд стоит на станции)
                {
                    var j = StationQueues.Count - train.getTimer() / 20 - 1; // j - номер станции, на которой находится поезд

                    int number1 = random.Next(0, train.getPassengersLoad()); // Генерация случайного числа высаживающихся пассажиров
                    for (int k = 0; k < number1; k++) // Цикл по числу высаживающихся пассажиров
                    {
                        train.popPassenger(); // Высадка пассажиров
                    }

                    int max = Math.Min(train.getMaxCapacity(), StationQueues[j].getSize()); // Максимальное количество пассажиров, которое может войти в поезд
                    int number2 = random.Next(0, max); // Генерация случайного числа пассажиров
                    for (int k = 0; k < number2; k++) // Цикл по числу пассажиров
                    {
                        train.pushPassenger((Person)StationQueues[j].pop()); // Посадка пассажиров
                    }
                }

                if (train.getTimer() <= 0)
                {
                    for (int j = 0; j < train.getPassengersLoad(); j++) // Цикл по числу пассажиров в поезде
                    {
                        train.popPassenger(); // Высадка всех пассажиров
                    }
                    Trains.RemoveAt(i); // Удаление поезда из списка
                }
            }

            if (timeOfWork % freqTrains == 0) // Если время работы кратно частоте прибытия поездов
            {
                Train newTrain = new Train(numberOfStations * 20, capacity); // Создание нового поезда
                Trains.Add(newTrain); // Добавление поезда в список
            }

            totalSize = 0; // Общий размер
            for (int i = 0; i < numberOfStations; i++) // Цикл по числу станций
            {
                this.chart1.Series[0].Points.AddXY(i, StationQueues[i].getSize()); 
                totalSize += StationQueues[i].getSize(); // Увеличение общего размера на размер очереди
            }
            this.chart2.Series[0].Points.AddXY(timeOfWork, totalSize);

            totalSize = 0;
            for (int i = 0; i < Trains.Count; i++) // Цикл по числу поездов
            {
                Train train = Trains[i]; // Получение текущего поезда
                totalSize += train.getPassengersLoad(); // Увеличение общего размера на количество пассажиров в поезде
            }
            this.chart3.Series[0].Points.AddXY(timeOfWork, totalSize);
        }

        private int timeOfWork; // Время работы метро в тиках
        private int numberOfStations; // Число станций (очередей)
        private int freqTrains; // Время между составами на первой станции в тиках
        private int capacity; // Максимальная вместимость вагона
        private int maxNumberOfPeople; // ВВОДИМОЕ число прибывающих людей
        private int numberOfPeople; // РАНДОМНОЕ от 0 до МАКС число прибывающих людей
        private Random random = new Random();
        private int totalSize; // Общий размер

        private void button1_Click(object sender, EventArgs e) 
        {
            numberOfStations = Convert.ToInt32(textBox1.Text);
            StationQueues = new List<MyQueue>(numberOfStations); // Инициализация списка очередей станций
            Trains = new List<Train>(1000); // Инициализация списка поездов
            for (int i = 0; i < numberOfStations; i++)
            {
                MyQueue newStation = new MyQueue(); // Создание новой очереди станции
                StationQueues.Add(newStation); // Добавление очереди станции в список
            }
            freqTrains = Convert.ToInt32(textBox2.Text); 
            capacity = Convert.ToInt32(textBox3.Text); 
            maxNumberOfPeople = Convert.ToInt32(textBox4.Text);
            Train firstTrain = new Train(numberOfStations * 20, capacity); // Создание первого поезда
            Trains.Add(firstTrain); 
            timeOfWork = 0; // Обнуление времени работы
            this.chart1.Series[0].Points.Clear();
            this.chart2.Series[0].Points.Clear();
            this.chart3.Series[0].Points.Clear();
            for (int i = 0; i < numberOfStations; i++) // Цикл по числу станций
            {
                this.chart1.Series[0].Points.AddXY(i, 0); 
            }
            this.chart2.Series[0].Points.AddXY(0, 0); 
            timer1.Start(); 
        }

        private void button2_Click(object sender, EventArgs e) // Остановка
        {
            timer1.Stop(); 
        }
    }
    public class Person //Класс человека
    {
        private MyQueue queue;
        public MyQueue getQueue()
        {
            return queue;
        }
        public void setQueue(MyQueue queue)
        {
            this.queue = queue;
        }
    }
    public class Train //Поезд
    {
        private int timer;
        public void setTimer(int timer)
        {
            this.timer = timer;
        }
        public int getTimer()
        {
            return this.timer;
        }
        public void decTime()
        {
            this.timer--;
        }
        private MyQueue passengers;
        public void setPassengers(MyQueue passengers)
        {
            this.passengers = passengers;
        }
        public void pushPassenger(Person person)
        {
            this.passengers.push(person);
        }
        public Person popPassenger()
        {
            return (Person)this.passengers.pop();
        }
        private int maxCapacity;
        public void setMaxCapacity(int maxCapacity)
        {
            this.maxCapacity = maxCapacity;
        }
        public int getMaxCapacity()
        {
            return this.maxCapacity;
        }
        public int getPassengersLoad()
        {
            return this.passengers.getSize();
        }
        public Train(int timer, int capacity)
        {
            this.timer = timer;
            this.passengers = new MyQueue();
            this.maxCapacity=capacity;
        }
    }
}
