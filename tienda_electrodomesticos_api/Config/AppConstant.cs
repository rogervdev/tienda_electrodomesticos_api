namespace tienda_electrodomesticos_api.Config
{
    public static class AppConstant
    {
        // Número máximo de intentos fallidos antes de bloquear la cuenta
        public const int ATTEMPT_LIMIT = 5;

        // Tiempo de desbloqueo de cuenta en minutos
        public const int UNLOCK_DURATION_MINUTES = 15;
    }
}
