using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiSD5
{
    public class MyQueue //Очередь
    {
        private QueueNode last;
        private QueueNode top;
        private int counter;
        public int getSize()
        {
            return counter;
        }
        public MyQueue()
        {
            this.top = null;
            this.last = null;
            this.counter = 0;
        }
        public MyQueue(object data) //Конструктор
        {
            top = new QueueNode(data);
            last = top;
            top.setPrev(null);
            this.counter = 1;
        }
        public void push(object data) //Push
        {
            QueueNode newNode = new QueueNode(data);
            if (top != null)
            {
                top.setPrev(newNode);
            }
            else
            {
                this.last = newNode;
            }
            top = newNode;
            this.counter++;
        }
        public object pop() //Pop
        {
            object result = last.getData();
            if (last == top)
            {
                this.last = null;
                this.top = null;
                this.counter = 0;
            }
            else
            {
                this.last = last.getPrev();
                this.counter--;
            }
            GC.Collect();
            return result;
        }
        public bool isEmpty() //Проверка пустоты
        {
            return last == null;
        }
        public void clear()
        {
            last = null;
            top = null;
            counter = 0;
        }
        public object getLast()
        {
            return this.last.getData();
        }
    }
    public class QueueNode //Элемент
    {
        private QueueNode Prev;
        public void setPrev(QueueNode prev) //Установка ссылки на пред элемент
        {
            this.Prev = prev;
        }
        public QueueNode getPrev() //Получение ссылки на пред элемент
        {
            return this.Prev;
        }
        private MyQueue parQueue;
        private object data;
        public object getData() //Получение данных
        {
            return this.data;
        }

        public void setQueue(MyQueue parQueue) //Установка ссылки на саму очередь
        {
            this.parQueue = parQueue;
        }
        public MyQueue GetMyQueue() //Получение ссылки на саму очередь
        {
            return this.parQueue;
        }
        public QueueNode(object data) //Конструктор
        {
            this.data = data;
        }
    }

}
