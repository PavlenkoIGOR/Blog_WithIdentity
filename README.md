1. База данных Sqlite;
2. Взаимодействие с БД через EntityFramework;
3. Авторизация настроена через тот же EF;
4. Добавлен API, Swagger;

godot
```C#
public class Obstacle : Node3D
{
        private sbyte _direction {get; set;} = 1;

        [Export]
        public double Speed = 140;

        public override void _Update()
        {
                // Calculating a start position of the Obstacle on the X-axis
                _startPosition = GlobalTransform.Origin;
        }
        public override void _Process(double delta)
        {
                // Calculating a new position of X-axis
                _startPosition.X = _startPosition.X + (float)(Speed * delta) * _direction;
                // If the target position is reached (30 or -30 relative to 0), we change the direction of movement
                if (_direction == 1 && _startPosition.X <= 20)
                {
                    _direction *= 1;
                    GD.Print($"_startPosition.X: {_startPosition.X}");
                }
                if (_startPosition.X == 20)
                {
                    _direction = 1;
                }
                if (_direction == -1 && _startPosition.X <= -20)
                {
                    _direction *= -1;
                    GD.Print($"_startPosition.X: {_startPosition.X}");
                }
                if (_startPosition.X == -20)
                {
                    _direction = -1;
                }

                // Accept updated position to global Transform
                Transform3D newTransform = Transform3D.Identity;
                newTransform.Origin = _startPosition;
                GlobalTransform = newTransform;
        }
}
```
