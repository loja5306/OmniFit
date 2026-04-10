resource "aws_instance" "main" {
  ami = data.aws_ami.amazon_linux.id
  instance_type = "t4g.small"
  subnet_id = aws_subnet.public.id
  vpc_security_group_ids = [aws_security_group.ec2.id]
  key_name = aws_key_pair.main.key_name
  iam_instance_profile = aws_iam_instance_profile.ec2.name
  user_data = file("scripts/user-data.sh")

  tags = {
    Name = "omnifit-ec2"
  }
}

resource "aws_eip" "main" {
  domain = "vpc"
  instance = aws_instance.main.id

  tags = {
    Name = "omnifit-eip"
  }
}

resource "aws_key_pair" "main" {
  key_name   = "omnifit-key"
  public_key = var.ec2_public_key
}

data "aws_ami" "amazon_linux" {
  most_recent = true
  owners      = ["amazon"]

  filter {
    name   = "name"
    values = ["al2023-ami-*-arm64"]
  }
}