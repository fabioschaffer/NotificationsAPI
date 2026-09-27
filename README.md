Criar imagem da api para Docker:
	docker build -t notifications-api:1.0 .
	
Executar imagem:
	docker run -p 8082:8080 notifications-api:1.0
	
Abrir a aplicação:
	http://localhost:8082/swagger/index.html