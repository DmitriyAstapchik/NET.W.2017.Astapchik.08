# NET.W.2017.Astapchik.08
EPAM training day 8 homework
1. Разработать класс Book (ISBN, автор, название, издательство, год издания, количество страниц, цена), переопределив для него необходимые методы класса Object. Для объектов класса реализовать отношения эквивалентности и порядка (используя соответствующие интерфейсы). Для выполнения основных операций со списком книг, который можно загрузить и, если возникнет необходимость, сохранить в некоторое хранилище BookListStorage разработать класс BookListService (как сервис для работы с коллекцией книг) с функциональностью AddBook (добавить книгу, если такой книги нет, в противном случае выбросить исключение); RemoveBook (удалить книгу, если она есть, в противном случае выбросить исключение); FindBookByTag (найти книгу по заданному критерию); SortBooksByTag (отсортировать список книг по заданному критерию), при реализации делегаты не использовать!

Работу классов продемонстрировать на примере консольного приложения.

В качестве хранилища использовать двоичный файл, для работы с которым использовать только классы BinaryReader, BinaryWriter. Хранилище в дальнейшем может измениться/добавиться.

2.  Разработать систему типов для описания работы с банковским счетом. Состояние счета определяется его номером, данными о владельце счета (имя, фамилия), суммой на счете и некоторыми бонусными баллами, которые увеличиваются/уменьшаются каждый раз при пополнении счета/списании со счета на величины различные для пополнения и списания и рассчитываемые в зависимости от некоторых значений величин «стоимости» баланса и «стоимости» пополнения. Величины «стоимости» баланса и «стоимости» пополнения являются целочисленными значениями и зависят от градации счета, который может быть, например,  Base, Gold, Platinum.

Для работы со счетом реализовать следующие возможности: 
- выполнить пополнение на счет;
- выполнить списание со счета; 
- создать новый счет; 
- закрыть счет. 

Информация о счетах храниться в бинарном файле.

Работу классов продемонстрировать на примере консольного приложения. 

При проектировании типов учитывать следующие возможности расширения/изменения функциональности
- добавление новых видов счетов;
- изменение/добавление источников хранения информации о счетах;
- изменение логики расчета бонусных баллов.
