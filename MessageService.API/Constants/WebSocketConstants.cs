namespace MessageService.API.Constants
{
    public class WebSocketConstants
    {
        //Начальная позиция в массиве buffer, откуда начинается чтение байтов для преобразования в строку
        public const int BufferStartIndex = 0;

        // Размер буфера для получения данных через WebSocket (4 КБ)
        public const int BufferSize = 1024 * 4; 
    }
}
